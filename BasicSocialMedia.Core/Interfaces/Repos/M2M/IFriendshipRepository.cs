using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Core.Interfaces.Repos.M2M
{
	public interface IFriendshipRepository : IBaseRepository<Friendship>
	{
		Task<IEnumerable<Friendship?>> GetAllAsync(string userId);
	}
}
