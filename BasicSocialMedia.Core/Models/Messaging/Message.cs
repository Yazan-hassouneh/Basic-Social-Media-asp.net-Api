using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Core.Models.Messaging
{
	public class Message : IId, ITimestamp, IContent
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? Content { get; set; }
		public virtual IEnumerable<MessageFileModel> Files { get; set; } = new HashSet<MessageFileModel>();
		public bool IsRead { get; set; } = false;
		public bool IsDeleted { get; set; } = false;
		public int ChatId { get; set; }
		public Chat? Chat { get; set; }
		public string? SenderId { get; set; } 
		public string? ReceiverId { get; set; }
		public virtual ICollection<DeletedMessage> DeletedByUsers { get; set; } = new HashSet<DeletedMessage>();


	}
}
