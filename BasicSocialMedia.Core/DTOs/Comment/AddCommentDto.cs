using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class AddCommentDto : IContentDto, IUserIdDto, IPostIdDto
	{
		public string Content { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public int PostId { get; set; }
	}
}
