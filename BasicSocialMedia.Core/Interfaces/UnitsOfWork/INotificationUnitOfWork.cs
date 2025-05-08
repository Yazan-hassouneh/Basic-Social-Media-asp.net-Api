using BasicSocialMedia.Core.Interfaces.Repos.NotificationsRepos;
using Microsoft.EntityFrameworkCore.Storage;

namespace BasicSocialMedia.Core.Interfaces.UnitsOfWork
{
	public interface INotificationUnitOfWork : IDisposable
	{
		public INewCommentNotificationRepository NewCommentNotifications{ get; }
		public INewFollowerNotificationRepository NewFollowerNotifications{ get; }
		public IPostReactionNotificationRepository PostReactionNotifications { get; }
		public ICommentReactionNotificationRepository CommentReactionNotifications { get; }
		public IFriendRequestNotificationRepository FriendRequestNotifications { get; }
		Task<IDbContextTransaction> BeginTransactionAsync();
	}
}
