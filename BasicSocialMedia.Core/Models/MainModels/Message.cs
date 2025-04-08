using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Message : IChatMessage, IContent
	{
		public int Id { get ; set ; }
		public DateTime CreatedOn { get ; set ; }
		public string? Content { get; set; }
		public virtual IEnumerable<MessageFileModel> Files { get; set; } = [];
		public bool IsRead { get; set; } = false;
		public int ChatId { get; set; }
		public Chat? Chat { get; set; }
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;

	}
}
