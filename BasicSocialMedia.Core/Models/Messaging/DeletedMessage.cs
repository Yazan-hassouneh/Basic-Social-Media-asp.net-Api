using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.Messaging
{
	public class DeletedMessage : IId
	{
		public int Id { get; set; }
		public int MessageId { get; set; }
		public virtual Message? Message { get; set; }
		public int ChatId { get; set; }
		public virtual Chat? Chat { get; set; }
		public string UserId { get; set; } = null!;
		public virtual ApplicationUser? User { get; set; }
	}
}
