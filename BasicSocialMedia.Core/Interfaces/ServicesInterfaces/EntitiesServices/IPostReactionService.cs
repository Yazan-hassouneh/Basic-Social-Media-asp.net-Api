using BasicSocialMedia.Core.DTOs.ReactionsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IPostReactionService
	{
		Task<IEnumerable<GetReactionDto>> GetPostReactionsByPostIdAsync(int postId);
		Task CreatePostReactionAsync(AddPostReactionDto postReactionDto);
		Task<bool> DeletePostAsync(int reactionId);
	}
}
