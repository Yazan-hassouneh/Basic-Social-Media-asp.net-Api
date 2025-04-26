using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos
{
	public interface IMessageRepository : IBaseRepository<Message>
	{
		Task<string?> GetUserId(int messageId);
		Task<Message?> MessageExist(int messageId);
		Task<Message?> GetByIdAsync(int id, string userId);
		Task<IEnumerable<Message?>> GetAllAsync(int chatId, string userId);
	}
}
