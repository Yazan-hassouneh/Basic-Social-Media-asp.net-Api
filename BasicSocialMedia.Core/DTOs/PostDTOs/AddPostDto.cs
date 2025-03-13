using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class AddPostDto : IContentDto, IAudienceDto
	{
		public string Content { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public int Audience { get; set; }
	}
}
