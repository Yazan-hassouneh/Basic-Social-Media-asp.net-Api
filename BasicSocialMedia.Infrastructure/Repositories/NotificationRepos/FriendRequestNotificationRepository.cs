using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.NotificationRepos
{
	internal class FriendRequestNotificationRepository(ApplicationDbContext context) : BaseNotificationRepository<FriendRequestNotification>(context), IFriendRequestNotificationRepository
	{
	}
}
