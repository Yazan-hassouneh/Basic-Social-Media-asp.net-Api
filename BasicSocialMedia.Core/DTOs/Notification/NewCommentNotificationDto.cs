using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.Notification
{
	public class NewCommentNotificationDto
	{
		public int Id { get; set; }
		public string NotificationType { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public string NotifiedUserId { get; set; } = null!;
		public int PostId { get; set; }
		public int CommentId { get; set; }
		public string UserId { get; set; } = null!;
		public GetBasicUserInfo? User { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
