using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.Notification
{
	public class FriendRequestNotification : INotification
	{
		public int Id { get; set; }
		public string NotifiedUserId { get; set; } = null!;
		public string NotificationType { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public string UserId { get; set; } = null!;
		public ApplicationUser? User { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
