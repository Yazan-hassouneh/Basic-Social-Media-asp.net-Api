using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.NotificationRepos
{
	internal class UserNotificationRepository(ApplicationDbContext context) : BaseNotificationRepository<UserNotification>(context), IUserNotificationRepository
	{
	}
}
