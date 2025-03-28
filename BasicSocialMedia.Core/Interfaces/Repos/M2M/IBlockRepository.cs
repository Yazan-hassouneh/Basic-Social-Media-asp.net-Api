using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Core.Interfaces.Repos.M2M
{
	public interface IBlockRepository : IBaseRepository<Block>
	{
		Task<bool> IsBlockingAsync(string userId1, string userId2);
		Task<IEnumerable<Block?>> GetBlockListAsync(string userId);
	}
}
