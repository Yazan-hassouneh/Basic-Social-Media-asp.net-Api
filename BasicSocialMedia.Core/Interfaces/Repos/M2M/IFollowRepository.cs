using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Core.Interfaces.Repos.M2M
{
	public interface IFollowRepository : IBaseRepository<Follow>
	{
		Task<IEnumerable<Follow?>> GetAllFollowersAsync(string userId);
		Task<IEnumerable<string>> GetAllFollowingsIdsAsync(string userId);
		Task<IEnumerable<Follow?>> GetAllFollowingsAsync(string userId);
	}
}
