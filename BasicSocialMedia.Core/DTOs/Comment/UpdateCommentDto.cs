using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class UpdateCommentDto : IContentDto
	{
		public string Content { get; set; } = null!;
	}
}
