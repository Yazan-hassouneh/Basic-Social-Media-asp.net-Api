
namespace BasicSocialMedia.Core.DTOs.Notification
{
	public class GetNotificationDto
	{
		public required string UserId { get; set; } = null!;
		public IEnumerable<CommentReactionNotificationDto> CommentReactionNotifications { get; set; } = [];
		public IEnumerable<PostReactionNotificationDto> PostReactionNotifications { get; set; } = [];
		public IEnumerable<NewCommentNotificationDto> NewCommentNotifications { get; set; } = [];
		public IEnumerable<NewFollowerNotificationDto> NewFollowerNotifications { get; set; } = [];
		public IEnumerable<FriendRequestNotificationDto> FriendRequestNotifications { get; set; } = [];
	}
}
