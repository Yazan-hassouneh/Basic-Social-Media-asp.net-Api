using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;
using System.Linq.Expressions;

namespace BasicSocialMedia.Core.Interfaces.Repos
{
	public interface IPostRepository : IBaseRepository<Post>
	{
		Task<string?> GetUserId(int postId);
		Task<IEnumerable<Post?>> GetAllAsync(Expression<Func<Post, bool>> matcher);
		Task<IEnumerable<Post?>> GetAllAsync(string userId);
	}
}
