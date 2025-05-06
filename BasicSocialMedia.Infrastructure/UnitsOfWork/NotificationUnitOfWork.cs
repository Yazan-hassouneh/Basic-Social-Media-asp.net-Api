using BasicSocialMedia.Core.Interfaces.UnitsOfWork;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using BasicSocialMedia.Infrastructure.Repositories.NotificationRepos;

namespace BasicSocialMedia.Infrastructure.UnitsOfWork
{
	public class NotificationUnitOfWork : INotificationUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public IUserNotificationRepository UserNotifications { get; private set; }
		public INewCommentNotificationRepository NewCommentNotifications { get; private set; }
		public INewFollowerNotificationRepository NewFollowerNotifications { get; private set; }
		public IPostReactionNotificationRepository PostReactionNotifications { get; private set; }
		public IFriendRequestNotificationRepository FriendRequestNotifications { get; private set; }
		public ICommentReactionNotificationRepository CommentReactionNotifications { get; private set; }

		public NotificationUnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;

			UserNotifications = new UserNotificationRepository(_context);
			NewCommentNotifications = new NewCommentNotificationRepository(_context);
			NewFollowerNotifications = new NewFollowerNotificationRepository(_context);
			PostReactionNotifications = new PostReactionNotificationRepository(_context);
			FriendRequestNotifications = new FriendRequestNotificationRepository(_context);
			CommentReactionNotifications = new CommentReactionNotificationRepository(_context);
		}
		public async Task<int> Complete() => await _context.SaveChangesAsync();
		public Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return _context.Database.BeginTransactionAsync();
		}
		public void Dispose() => _context.Dispose();
	}
}
