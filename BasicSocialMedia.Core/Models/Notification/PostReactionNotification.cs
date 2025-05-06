using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.Notification
{
	public class PostReactionNotification : INotification
	{
		public int Id { get; set; }
		public string NotificationType { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public string NotifiedUserId { get; set; } = null!;
		public int PostId { get; set; }
		public string UserId { get; set; } = null!;
		public ApplicationUser? User { get; set; }
		public int UserNotificationId { get; set; }
		public UserNotification? UserNotification { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
