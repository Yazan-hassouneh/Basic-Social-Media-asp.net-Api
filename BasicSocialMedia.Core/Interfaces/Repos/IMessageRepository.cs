using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos
{
	public interface IMessageRepository : IBaseRepository<Message>
	{
		Task<IEnumerable<Message?>> GetAllAsync(int chatId);
	}
}
