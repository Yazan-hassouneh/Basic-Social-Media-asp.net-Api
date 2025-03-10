

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class AddCommentDto
	{
		public string Content { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public int PostId { get; set; }
	}
}
