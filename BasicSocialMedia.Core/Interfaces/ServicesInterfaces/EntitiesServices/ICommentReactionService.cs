using BasicSocialMedia.Core.DTOs.ReactionsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface ICommentReactionService
	{
		Task<IEnumerable<GetReactionDto>> GetCommentReactionsByCommentIdAsync(int commentId);
		Task CreateCommentReactionAsync(AddCommentReactionDto commentReactionDto);
		Task<bool> DeleteCommentReaction(int reactionId);
	}
}
