using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos.Reactions
{
	public interface ICommentReactionRepository : IBaseRepository<CommentReaction>
	{
		Task<IEnumerable<CommentReaction?>> GetAllAsync(int commentId);
		Task<string?> GetUserId(int commentReactionId);
	}
}
