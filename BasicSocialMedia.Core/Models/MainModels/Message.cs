using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Message : IChatMessage, IContent
	{
		public int Id { get ; set ; }
		public DateTime CreatedOn { get ; set ; }
		public string Content { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public int ChatId { get; set; }
		public Chat? Chat { get; set; }
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;

	}
}
