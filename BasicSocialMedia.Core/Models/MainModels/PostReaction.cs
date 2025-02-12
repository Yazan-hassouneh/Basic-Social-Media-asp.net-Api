using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class PostReaction : IReactions
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string UserId { get; set; } = null!;
		public virtual ApplicationUser? User { get; set; }
		public int PostId { get; set; }
		public virtual Post? Post { get; set; }
	}
}
