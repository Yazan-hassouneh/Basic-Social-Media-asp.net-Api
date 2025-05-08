using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;

namespace BasicSocialMedia.Application.Services.NotificationServices
{
	public class BaseNotificationService<T>(IBaseNotificationRepository<T> repository) : IBaseNotificationService<T> where T : class, INotification
	{
		private readonly IBaseNotificationRepository<T> _repository = repository;

		public async Task<string> GetNotifiedUserId(int id)
		{
			var result = await _repository.GetByIdAsync(id);
			if (result is not null) return result.NotifiedUserId;
			return string.Empty;
		}
		public async Task<IEnumerable<T>> GetAllAsync(string userId)
		{
			return await _repository.GetAllAsync(n => n.NotifiedUserId == userId);
		}

		public async Task<bool> MarkAsReadAsync(int id)
		{
			var item = await _repository.GetByIdWithTrackingAsync(id);
			if (item is not null)
			{
				item.IsRead = true;
				var result = _repository.Update(item);
				return result != null;
			}
			return false;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var item = await _repository.GetByIdWithTrackingAsync(id);
			if (item is not null)
			{
				_repository.Delete(item);
				return true;
			}
			return false;
		}

	}

}
