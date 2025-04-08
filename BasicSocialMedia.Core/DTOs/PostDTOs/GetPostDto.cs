using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.Comment;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class GetPostDto
	{
		public int Id { get; set; }
		public string? Content { get; set; }
		public List<string>? Files { get; set; }
		public int CommentsCount { get; set; }
		public int ReactionsCount { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? Audience { get; set; }
		public string UserId { get; set; } = null!;
		public string RowVersion { get; set; } = null!;
		public virtual GetBasicUserInfo? User { get; set; }
		public virtual List<GetCommentDto> Comments { get; set; } = [];
		public virtual List<GetBasicUserInfo> ReactionsList { get; set; } = [];
	}
}
