using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.Notification
{
	public class UserNotification : IId
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public ApplicationUser? User { get; set; }
		public virtual IEnumerable<NewCommentNotification>? NewCommentNotifications { get; set; }
		public virtual IEnumerable<NewFollowerNotification>? NewFollowerNotifications { get; set; }
		public virtual IEnumerable<PostReactionNotification>? PostReactionNotifications { get; set; }
		public virtual IEnumerable<CommentReactionNotification>? CommentReactionNotifications { get; set; }
		public virtual IEnumerable<FriendRequestNotification>? FriendRequestNotifications { get; set; }


	}
}
