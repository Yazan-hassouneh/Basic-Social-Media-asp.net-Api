using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos.Reactions
{
	public interface IPostReactionRepository : IBaseRepository<PostReaction>
	{
		Task<IEnumerable<PostReaction?>> GetAllAsync(int postId);
	}
}
