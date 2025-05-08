using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.NotificationRepos
{
	internal class NewCommentNotificationRepository(ApplicationDbContext context) : BaseNotificationRepository<NewCommentNotification>(context), INewCommentNotificationRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<IEnumerable<NewCommentNotification>> GetAllAsync(Expression<Func<NewCommentNotification, bool>> matcher)
		{
			return await _context.NewCommentNotifications
				.Where(matcher)
				.OrderByDescending(x => x.CreatedOn)
				.Include(x => x.User)
				.Select(notification => new NewCommentNotification
				{
					Id = notification.Id,
					NotificationType = notification.NotificationType,
					IsRead = notification.IsRead,
					NotifiedUserId = notification.NotifiedUserId,
					UserId = notification.UserId,
					CreatedOn = notification.CreatedOn,
					CommentId = notification.CommentId,
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
