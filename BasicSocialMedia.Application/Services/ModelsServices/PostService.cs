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

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class PostService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, HtmlSanitizer sanitizer) : IPostService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly HtmlSanitizer _sanitizer = sanitizer;

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
			IEnumerable<Post?> posts = await _unitOfWork.Posts.GetAllAsync(userId);// By default , GetAllAsync returns all posts that related to the user
			if (posts == null) return [];
			IEnumerable<Post> nonNullPosts = posts.Where(post => post != null)!;
			if (nonNullPosts == null || !nonNullPosts.Any()) return [];

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
				Content = _sanitizer.Sanitize(postDto.Content),
				UserId = postDto.UserId
			};

			await _unitOfWork.Posts.AddAsync(post);
			await _unitOfWork.Posts.Save();
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
				// Row versions don't match, indicating a concurrency conflict
				// You should handle this appropriately, e.g., throw an exception or return a specific error code
				throw new Exception("Concurrency conflict: The post has been modified by another user.");
			}

			post.Content = _sanitizer.Sanitize(postDto.Content);
			post.Audience = (PostAudience)postDto.Audience;

			_unitOfWork.Posts.Update(post);
			await _unitOfWork.Posts.Save();
			return postDto;
		}
		public async Task<bool> DeletePostAsync(int postId)
		{
			Post? post = await _unitOfWork.Posts.GetByIdAsync(postId);
			if (post == null) return false;
			_unitOfWork.Posts.Delete(post);
			await _unitOfWork.Posts.Save();
			return true;
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
	}
}
