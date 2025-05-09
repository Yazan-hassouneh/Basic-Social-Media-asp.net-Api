using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;
using Microsoft.AspNetCore.SignalR;

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
		public async Task<bool> AddNotificationAsync(T notification)
		{
			var result = await _repository.AddAsync(notification);
			if (result is not null)
			{
				var saveResult = await _repository.Save();
				if(saveResult > 0)
				{
					// Add REal Time Functionalities

					//var notification = new NotificationDto
					//{
					//	Id = 123,
					//	Type = "NewFollower",
					//	Message = $"User {followerId} followed you.",
					//	Timestamp = DateTime.UtcNow
					//};

					//await _hubContext.Clients.User(followedUserId.ToString())
					//	.SendAsync("ReceiveNotification", notification);

					return true;
				}
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
