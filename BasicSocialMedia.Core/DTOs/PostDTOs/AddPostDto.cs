using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.PostDTOs
{
	public class AddPostDto : IAudienceDto, IUserIdDto, ICreateFile
	{
		public string UserId { get; set; } = null!;
		public List<IFormFile> Files { get; set; } = [];
		public string? Content { get; set; }
		public int Audience { get; set; }
	}
}
