using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos
{
	public interface IChatRepository : IBaseRepository<Chat>
	{
		Task<IEnumerable<Chat?>> GetAllAsync(string userId);
	}
}
