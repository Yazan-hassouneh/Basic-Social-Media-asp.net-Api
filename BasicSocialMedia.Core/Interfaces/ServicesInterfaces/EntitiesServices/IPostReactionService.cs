using BasicSocialMedia.Core.DTOs.ReactionsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IPostReactionService
	{
		Task<string?> GetUserId(int postReactionId);
		Task<IEnumerable<GetReactionDto>> GetPostReactionsByPostIdAsync(int postId);
		Task<AddPostReactionDto> CreatePostReactionAsync(AddPostReactionDto postReactionDto);
		Task<bool> DeletePostReactionAsync(int reactionId);
		Task<bool> DeletePostReactionsByPostIdAsync(int postId);
		Task<bool> DeletePostReactionsByUserIdAsync(string userId);
	}
}
