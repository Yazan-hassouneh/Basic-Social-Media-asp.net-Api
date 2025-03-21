using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Comment : IPostComment
	{
		public int Id { get; set; }
		public string Content { get; set; } = null!;
		public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; }
		public string UserId { get; set; } = null!;
		public ApplicationUser? User { get; set ; }
		public int PostId { get; set; }
		public virtual Post? Post { get; set; }
		public virtual ICollection<CommentReaction> CommentReactions { get; set; } = [];
		[Timestamp]
		public byte[] RowVersion { get ; set; }
	}
}
