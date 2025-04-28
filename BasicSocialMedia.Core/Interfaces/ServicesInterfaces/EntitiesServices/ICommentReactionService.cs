using BasicSocialMedia.Core.DTOs.ReactionsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface ICommentReactionService
	{
		Task<IEnumerable<GetReactionDto>> GetCommentReactionsByCommentIdAsync(int commentId);
		Task<AddCommentReactionDto> CreateCommentReactionAsync(AddCommentReactionDto commentReactionDto);
		Task<bool> DeleteCommentReaction(int reactionId);
		Task<string?> GetUserId(int commentReactionId);
		Task<bool> DeleteCommentReactionsByCommentIdAsync(int commentId);
	}
}
