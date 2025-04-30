using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using AutoMapper;
using BasicSocialMedia.Core.Models.MainModels;
using Microsoft.AspNetCore.Identity;
using BasicSocialMedia.Core.Models.AuthModels;
using static BasicSocialMedia.Core.Enums.ProjectEnums;
using Microsoft.EntityFrameworkCore;
using Ganss.Xss;
using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class PostService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, HtmlSanitizer sanitizer, IPostFileModelService postFileModelService, IPostReactionService postReactionService, ICommentService commentService) : IPostService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly HtmlSanitizer _sanitizer = sanitizer;
		private readonly IPostFileModelService _postFileModelService = postFileModelService;
		private readonly IPostReactionService _postReactionService = postReactionService;
		private readonly ICommentService _commentService = commentService;

		/*
			✅ Prevents XSS attacks by removing harmful scripts
			✅ More customizable than WebUtility.HtmlEncode
			✅ Maintained & updated (unlike Microsoft.Security.Application.Sanitizer)
			✅ Retains safe HTML formatting (e.g., <b>, <i>, <p>, etc.)
		 */

		public async Task<GetPostDto?> GetPostByIdAsync(int postId)
		{
			Post? post = await _unitOfWork.Posts.GetByIdAsync(postId); 
			if (post == null) return null;
			GetPostDto postDto = _mapper.Map<GetPostDto>(post);
			return postDto;
		}
		public Task<string?> GetUserId(int postId)
		{
			return _unitOfWork.Posts.GetUserId(postId);
		}
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserIdAsync(string userId)
		{
			IEnumerable<Post?> posts = await _unitOfWork.Posts.GetAllAsync(userId); // By default, GetAllAsync returns all posts that are related to the user  
			if (posts == null) return Enumerable.Empty<GetPostDto>();

			IEnumerable<Post> nonNullPosts = posts.Where(post => post != null).Cast<Post>();
			if (!nonNullPosts.Any()) return Enumerable.Empty<GetPostDto>();

			IEnumerable<GetPostDto> postsDto = _mapper.Map<IEnumerable<GetPostDto>>(nonNullPosts);
			return postsDto;
		}
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserFollowingsAsync(string userId)
		{
			var user = await GetUser(userId);
			if (user == null) return Enumerable.Empty<GetPostDto>();

			IEnumerable<string> followingIds = await _unitOfWork.Following.GetAllFollowingsIdsAsync(userId);
			if (!followingIds.Any()) return Enumerable.Empty<GetPostDto>();

			return await MapPosts(followingIds.ToArray(), PostAudience.Public);
		}
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserFriendsAsync(string userId)
		{
			var user = await GetUser(userId);
			if (user == null) return Enumerable.Empty<GetPostDto>();

			IEnumerable<string> friendsIds = await _unitOfWork.Friendship.GetAllFriendsIdsAsync(userId);
			if (!friendsIds.Any()) return Enumerable.Empty<GetPostDto>();

			return await MapPosts(friendsIds.ToArray(), PostAudience.Friends);
		}
		public async Task<AddPostDto> CreatePostAsync(AddPostDto postDto)
		{
			Post post = new()
			{
				Comments = [],
				PostReactions = [],
				Audience = (PostAudience)postDto.Audience,
				Content = _sanitizer.Sanitize(postDto.Content ?? string.Empty),
				UserId = postDto.UserId
			};

			await _unitOfWork.Posts.AddAsync(post);
			int effectedRows = await _unitOfWork.Posts.Save();

			if (postDto.Files.Count > 0 && effectedRows > 0)
			{
				AddPostFileDto addPostFileDto = new()
				{
					UserId = postDto.UserId,
					PostId = post.Id,
					Files = postDto.Files
				};

				bool mediaSaved = await _postFileModelService.AddPostFileAsync(addPostFileDto);
				if (!mediaSaved)
				{
					_unitOfWork.Posts.Delete(post);
					await _unitOfWork.Posts.Save();
					return new AddPostDto();
				}
			}
			return postDto;
		}
		public async Task<UpdatePostDto?> UpdatePostAsync(UpdatePostDto postDto)
		{
			CancellationToken cancellationToken = new();
			bool isPostExist = await _unitOfWork.Posts.DoesExist(postDto.Id, cancellationToken);
			if (!isPostExist) return null;

			Post? post = await _unitOfWork.Posts.GetByIdWithTrackingAsync(postDto.Id);
			if (post == null) return null;

			byte[] providedRowVersionBytes = Convert.FromBase64String(postDto.RowVersion);
			if (!Compare.ByteArrayCompare(post.RowVersion, providedRowVersionBytes))
			{
				throw new DbUpdateConcurrencyException("Concurrency conflict: The post has been modified by another user.");
			}

			bool hasNewFiles = postDto.Files is not null && postDto.Files.Count > 0;
			List<string>? oldMediaPath = postDto.MediaPaths ?? [];
			IEnumerable<PostFileModel> postFileModels = (await _unitOfWork.PostFiles.FindAllAsync(file => file.PostId == postDto.Id)).Where(file => file != null)!;

			UpdatePostFileDto? updatePostFileDto = null;
			if (hasNewFiles || oldMediaPath!.Count > 0)
			{
				updatePostFileDto = new UpdatePostFileDto()
				{
					UserId = postDto!.UserId,
					PostId = post.Id,
					Files = postDto.Files,
					MediaPaths = oldMediaPath
				};
			}

			post.Content = _sanitizer.Sanitize(postDto?.Content ?? string.Empty);
			post.Audience = (PostAudience)postDto?.Audience!;
			_unitOfWork.Posts.Update(post);
			int effectedRows = await _unitOfWork.Posts.Save();

			if (effectedRows > 0)
			{
				if ( updatePostFileDto != null)
				{
					await _postFileModelService.UpdatePostFileAsync(updatePostFileDto);
				}
				if(updatePostFileDto is null && postFileModels.Any())
				{
					await _postFileModelService.DeletePostFileByPostIdAsync(postDto.Id);
				}
			}
			return postDto;
		}
		public async Task<bool> DeletePostAsync(int postId)
		{
			Post? post = await _unitOfWork.Posts.GetByIdWithTrackingAsync(postId);
			if (post == null) return false;
			List<string> files = await GetPostFiles(post.Id);

			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
			{
				// First delete related entities
				bool isRelatedEntitiesDeleted = await IsRelatedEntitiesDeleted(files, postId);
				if (!isRelatedEntitiesDeleted)
				{
					await transaction.RollbackAsync();
					return false;
				}

				// Then delete the post
				_unitOfWork.Posts.Delete(post);
				int effectedRows = await _unitOfWork.Posts.Save();

				if (effectedRows == 0) throw new Exception("Post deletion failed.");

				await transaction.CommitAsync();
				return true;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return false;
			}
		}
		public async Task<bool> DeleteAllPostsByUserIdAsync(string userId)
		{
			IEnumerable<Post?> posts = await _unitOfWork.Posts.FindAllWithTrackingAsync(post => post.UserId == userId);
			IEnumerable<Post> nonNullablePosts = posts.Where(post => post != null).Cast<Post>();

			using var transaction = await _unitOfWork.BeginTransactionAsync();
			try
			{
				foreach (var post in nonNullablePosts)
				{
					List<string> files = await GetPostFiles(post.Id);
					bool isRelatedEntitiesDeleted = await IsRelatedEntitiesDeleted(files, post.Id);
					if (!isRelatedEntitiesDeleted)
					{
						await transaction.RollbackAsync();
						return false;
					}

					_unitOfWork.Posts.Delete(post);
					int effected = await _unitOfWork.Posts.Save();
					if (effected == 0)
					{
						await transaction.RollbackAsync();
						return false;
					}
				}

				await transaction.CommitAsync();
				return true;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				// Log error here if needed
				throw; // or handle nicely
			}
		}

		// Helper Functions
		// 
		private async Task<ApplicationUser?> GetUser(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}		
		private async Task<IEnumerable<GetPostDto>> MapPosts(string[] Ids, PostAudience audience)
		{
			if (Ids.Length == 0) return Enumerable.Empty<GetPostDto>();
			var posts = await _unitOfWork.Posts.GetAllAsync(post => Ids.Contains(post.UserId) && (post.Audience == PostAudience.Public || post.Audience == audience));
			return _mapper.Map<IEnumerable<GetPostDto>>(posts);
		}
		private async Task<bool> IsRelatedEntitiesDeleted(List<string> files, int postId)
		{
			// The Files Entities Remove Automatically By The Database.
			bool isFileDeleted = _postFileModelService.DeletePostFiles(files); // filesystem, so maybe handle errors separately
			bool isReactionsDeleted = await _postReactionService.DeletePostReactionsByPostIdAsync(postId);
			bool isCommentsDeleted = await _commentService.DeleteCommentsByPostIdAsync(postId);

			if (!isCommentsDeleted || !isReactionsDeleted || !isFileDeleted)
			{
				return false;
			}
			return true;
		}
		private async Task<List<string>> GetPostFiles(int postId)
		{
			return  (await _unitOfWork.PostFiles.GetAllAsync(file => file.PostId == postId)).Select(file => file!.Path).ToList();

		}
		
	}
}
