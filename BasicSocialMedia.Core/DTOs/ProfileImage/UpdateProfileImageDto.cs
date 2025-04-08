using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.ProfileImage
{
	public class UpdateProfileImageDto : IUserIdDto
	{
		public string UserId { get; set; } = null!;
		public IFormFile? Image { get; set; } 
		public string? ImagePath { get; set; }
	}
}
