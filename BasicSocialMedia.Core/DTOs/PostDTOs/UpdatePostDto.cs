using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class UpdatePostDto : IAudienceDto, IUpdateFile, IUserIdDto
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public List<string>? MediaPaths { get; set; }
		public List<IFormFile> Files { get; set; } = [];
		public string? Content { get; set; }
		public int Audience { get; set; }
		public string RowVersion { get; set; } = null!;// Required for concurrency handling
	}
}
