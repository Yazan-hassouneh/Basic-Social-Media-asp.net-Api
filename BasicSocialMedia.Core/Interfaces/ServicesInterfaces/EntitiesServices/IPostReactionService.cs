using BasicSocialMedia.Core.DTOs.ReactionsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IPostReactionService
	{
		Task<IEnumerable<GetReactionDto>> GetPostReactionsByPostIdAsync(int postId);
		Task<AddPostReactionDto> CreatePostReactionAsync(AddPostReactionDto postReactionDto);
		Task<bool> DeletePostReactionAsync(int reactionId);
	}
}
