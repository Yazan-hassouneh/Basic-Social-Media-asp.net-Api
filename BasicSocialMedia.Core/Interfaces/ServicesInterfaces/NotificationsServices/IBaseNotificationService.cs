using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices
{
	public interface IBaseNotificationService<T> where T : class, INotification
	{
		Task<IEnumerable<T>> GetAllAsync(string userId);
		Task<string> GetNotifiedUserId(int id);
		Task<bool> MarkAsReadAsync(int id);
		Task<bool> DeleteAsync(int id);

	}
}
