using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using System.ComponentModel.DataAnnotations;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Post : IPostComment
	{
		public int Id { get; set; }
		public string? Content { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } = null!;
		public PostAudience Audience { get; set; } = PostAudience.Public;
		public string UserId { get; set; } = null!;
		public virtual ApplicationUser? User { get; set; }
		public virtual ICollection<Comment> Comments { get; set; } = [];
		public virtual IEnumerable<PostFileModel> Files { get; set; } = new HashSet<PostFileModel>();
		public virtual IEnumerable<CommentFileModel> CommentFiles { get; set; } = new HashSet<CommentFileModel>();
		public virtual ICollection<PostReaction> PostReactions { get; set; } = [];
	}
}
