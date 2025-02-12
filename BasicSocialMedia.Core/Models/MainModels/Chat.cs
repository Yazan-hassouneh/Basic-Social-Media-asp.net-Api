using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Chat : IChatMessage
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string User1Id { get; set; } = null!;
		public virtual ApplicationUser? User1 { get; set; }
		public string User2Id { get; set; } = null!;
		public virtual ApplicationUser? User2 { get; set; }
		public virtual ICollection<Message> Messages { get; set; } = [];
	}
}
