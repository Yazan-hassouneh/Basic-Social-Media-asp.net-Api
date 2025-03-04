using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IAccountService
	{
		Task<AuthDto> RegisterAsync(CreateAccountDto model);
	}
}
