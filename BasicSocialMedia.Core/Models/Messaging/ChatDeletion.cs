using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;

namespace BasicSocialMedia.Core.Models.Messaging
{
	public class ChatDeletion : IId
	{
		public int Id { get; set; }
		public int ChatId { get; set; }
		public string UserId { get; set; } = null!;
		public DateTime DeletedAt { get; set; }

		public Chat Chat { get; set; } = null!;
	}
}
