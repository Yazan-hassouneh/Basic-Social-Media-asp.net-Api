using BasicSocialMedia.Core.DTOs.PostDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IPostService
	{
		Task<IEnumerable<GetPostDto>> GetPostsByUserIdAsync(string userId);
		Task<IEnumerable<GetPostDto>> GetPostsByUserFollowingsAsync(string userId);
		Task<IEnumerable<GetPostDto>> GetPostsByUserFriendsAsync(string userId);
		Task<AddPostDto> CreatePostAsync(AddPostDto postDto);
		Task<UpdatePostDto?> UpdatePostAsync(UpdatePostDto postDto);
		Task<bool> DeletePostAsync(int commentId);
	}
}
