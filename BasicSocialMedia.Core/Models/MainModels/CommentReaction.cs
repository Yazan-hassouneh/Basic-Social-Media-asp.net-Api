using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class CommentReaction : IReactions
	{
		public int Id { get ; set ; }
		public DateTime CreatedOn { get ; set ; }
		public string? UserId { get; set; }
		public virtual ApplicationUser? User { get; set; }
		public int CommentId { get; set; }
		public virtual Comment? Comment { get; set; }
	}
}
