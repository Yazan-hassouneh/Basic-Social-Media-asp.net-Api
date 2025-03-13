using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class UpdatePostDto : IContentDto, IAudienceDto
	{
		public string Content { get; set; } = null!;
		public int Audience { get; set; }
	}
}
