using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.NotificationRepos
{
	internal class PostReactionNotificationRepository(ApplicationDbContext context) : BaseNotificationRepository<PostReactionNotification>(context), IPostReactionNotificationRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<IEnumerable<PostReactionNotification>> GetAllAsync(Expression<Func<PostReactionNotification, bool>> matcher)
		{
			return await _context.PostReactionNotifications
				.Where(matcher)
				.OrderByDescending(x => x.CreatedOn)
				.Include(x => x.User)
				.Select(notification => new PostReactionNotification
				{
					Id = notification.Id,
					NotificationType = notification.NotificationType,
					IsRead = notification.IsRead,
					NotifiedUserId = notification.NotifiedUserId,
					UserId = notification.UserId,
					CreatedOn = notification.CreatedOn,
					PostId = notification.PostId,
					User = notification.User != null ? new ApplicationUser
					{
						Id = notification.User.Id,
						UserName = notification.User.UserName,
						ProfileImageModel = notification.User.ProfileImageModel,
					} : null,
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
