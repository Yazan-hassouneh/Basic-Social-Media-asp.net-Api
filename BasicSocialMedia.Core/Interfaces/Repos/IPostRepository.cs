using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos
{
	public interface IPostRepository : IBaseRepository<Post>
	{
		Task<IEnumerable<Post?>> GetAllAsync(string userId);
	}
}
