using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Core.Models.MainModels
{
	public class Post : IPostComment
	{
		public int Id { get; set; }
		public string Content { get; set; } = null!;
		public uint LikesCount { get; set; }
		public int CommentsCount { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; }
		public PostAudience Audience { get; set; } = PostAudience.Public;
		public string UserId { get; set; } = null!;
		public virtual ApplicationUser? User { get; set; }
		public virtual ICollection<Comment> Comments { get; set; } = [];
		public virtual ICollection<PostReaction> PostReactions { get; set; } = [];
	}
}
