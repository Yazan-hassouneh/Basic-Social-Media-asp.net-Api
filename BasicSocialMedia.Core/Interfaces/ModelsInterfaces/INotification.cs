using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.Notification;

namespace BasicSocialMedia.Core.Interfaces.ModelsInterfaces
{
	public interface INotification : IId, ITimestamp
	{
		public string NotificationType { get; set; }
		public bool IsRead { get; set; }
		public string NotifiedUserId { get; set; }
		public string UserId { get; set; }
		public ApplicationUser? User { get; set; }
	}
}
