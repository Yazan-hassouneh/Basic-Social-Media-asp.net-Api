using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using AutoMapper;
using BasicSocialMedia.Core.Models.MainModels;
using Microsoft.AspNetCore.Identity;
using BasicSocialMedia.Core.Models.AuthModels;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	internal class PostService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : IPostService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserIdAsync(string userId)
		{
			IEnumerable<Post?> posts = await _unitOfWork.Posts.GetAllAsync(userId);
			IEnumerable<Post> nonNullPosts = posts.Where(post => post != null)!;
			if (nonNullPosts == null || !nonNullPosts.Any()) return [];

			IEnumerable<GetPostDto> postsDto = _mapper.Map<IEnumerable<GetPostDto>>(nonNullPosts);
			return postsDto;
		}
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserFollowingsAsync(string userId)
		{
			var user = await GetUser(userId);
			if (user == null) return Enumerable.Empty<GetPostDto>();

			var followingIds = user.Following
				.Where(f => !f.IsBlocked)
				.Select(f => f.FollowingId)
				.ToArray();

			return await MapPosts(followingIds);
		}
		public async Task<IEnumerable<GetPostDto>> GetPostsByUserFriendsAsync(string userId)
		{
			var user = await GetUser(userId);
			if (user == null) return Enumerable.Empty<GetPostDto>();

			var friendIds = user.Friendships
				.Where(f => f.Status == FriendshipStatus.Accepted)
				.Select(f => f.UserId1 == userId ? f.UserId2 : f.UserId1) // Get the friend's ID
				.ToArray();

			return await MapPosts(friendIds);
		}
		public async Task CreatePostAsync(AddPostDto postDto)
		{
			Post post = new()
			{
				Comments = [],
				PostReactions = [],
				Audience = (PostAudience)postDto.Audience,
				Content = postDto.Content,
				UserId = postDto.UserId
			};

			await _unitOfWork.Posts.AddAsync(post);
			await _unitOfWork.Posts.Save();
			await Task.CompletedTask;
		}
		public async Task UpdatePostAsync(UpdatePostDto postDto)
		{
			Post post = new()
			{
				Audience = (PostAudience)postDto.Audience,
				Content = postDto.Content,
			};

			_unitOfWork.Posts.Update(post);
			await _unitOfWork.Posts.Save();
			await Task.CompletedTask;
		}
		public async Task<bool> DeletePostAsync(int postId)
		{
			Post? post = await _unitOfWork.Posts.GetByIdAsync(postId);
			if (post == null) return false;
			_unitOfWork.Posts.Delete(post);
			await _unitOfWork.Posts.Save();
			return true;
		}
		private async Task<ApplicationUser?> GetUser(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}		
		private async Task<IEnumerable<GetPostDto>> MapPosts(string[] Ids)
		{
			if (Ids.Length == 0) return Enumerable.Empty<GetPostDto>();
			var posts = await _unitOfWork.Posts.FindAllAsync(post => Ids.Contains(post.UserId));
			return _mapper.Map<IEnumerable<GetPostDto>>(posts);
		}
	}
}
