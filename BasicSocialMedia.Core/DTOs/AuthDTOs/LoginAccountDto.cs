using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class LoginAccountDto : IPasswordDto, IEmailDto
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
