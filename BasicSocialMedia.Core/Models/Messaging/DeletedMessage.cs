using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;

namespace BasicSocialMedia.Core.Models.Messaging
{
	public class DeletedMessage : IId
	{
		public int Id { get; set; }
		public int MessageId { get; set; }
		public virtual Message? Message { get; set; }
		public int ChatId { get; set; }
		public virtual Chat? Chat { get; set; }
		public string? UserId { get; set; }
	}
}
