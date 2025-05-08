using BasicSocialMedia.Core.DTOs.AuthDTOs;
namespace BasicSocialMedia.Core.DTOs.Notification
{
	public class NewFollowerNotificationDto
	{
		public int Id { get; set; }
		public string NotifiedUserId { get; set; } = null!;
		public string NotificationType { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public string UserId { get; set; } = null!;
		public GetBasicUserInfo? User { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
