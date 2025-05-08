using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.NotificationRepos
{
	internal class CommentReactionNotificationRepository(ApplicationDbContext context) : BaseNotificationRepository<CommentReactionNotification>(context), ICommentReactionNotificationRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<IEnumerable<CommentReactionNotification>> GetAllAsync(Expression<Func<CommentReactionNotification, bool>> matcher)
		{
			return await _context.CommentReactionNotifications
				.Where(matcher)
				.OrderByDescending(x => x.CreatedOn)
				.Include(x => x.User)
				.Select(notification => new CommentReactionNotification
				{
					Id = notification.Id,
					NotificationType = notification.NotificationType,
					IsRead = notification.IsRead,
					NotifiedUserId = notification.NotifiedUserId,
					CommentId = notification.CommentId,
					PostId = notification.PostId,
					UserId = notification.UserId,
					CreatedOn = notification.CreatedOn,
					User = notification.User == null ? null : new ApplicationUser
					{
						Id = notification.User.Id,
						UserName = notification.User.UserName,
						ProfileImageModel = notification.User.ProfileImageModel,
					},

				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
