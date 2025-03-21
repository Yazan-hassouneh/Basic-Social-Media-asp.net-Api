using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class UpdatePostDto : IContentDto, IAudienceDto
	{
		public int Id { get; set; }
		public string Content { get; set; } = null!;
		public int Audience { get; set; }
		public string RowVersion { get; set; } = null!;// Required for concurrency handling
	}
}
