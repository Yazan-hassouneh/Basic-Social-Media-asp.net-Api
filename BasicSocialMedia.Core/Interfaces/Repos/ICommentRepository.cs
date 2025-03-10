using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos
{
	public interface ICommentRepository : IBaseRepository<Comment>
	{
		Task<IEnumerable<Comment?>> GetAllAsync(int postId);
	}
}
