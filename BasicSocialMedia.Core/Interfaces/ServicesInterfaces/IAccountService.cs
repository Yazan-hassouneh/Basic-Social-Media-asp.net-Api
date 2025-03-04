using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces
{
	public interface IAccountService
	{
		Task<AuthDto> RegisterAsync(CreateAccountDto model);
	}
}
