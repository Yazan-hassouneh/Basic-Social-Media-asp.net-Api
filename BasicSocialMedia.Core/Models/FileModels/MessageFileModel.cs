using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.Messaging;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Models.FileModels
{
	public class MessageFileModel : IFileModel
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int MessageId { get; set; }
		public virtual Message? Message { get; set; }
		public int ChatId { get; set; }
		public virtual Chat? Chat { get; set; }
		public string Path { get; set; } = null!;
		public DateTime CreatedOn { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } = null!;
	}
}
