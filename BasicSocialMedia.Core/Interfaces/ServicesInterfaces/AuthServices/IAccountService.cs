using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IAccountService
	{
		Task<AuthDto> RegisterAsync(CreateAccountDto model);
		Task<bool> TryCancelScheduledDeletionAsync(LoginAccountDto model);
		Task<bool> IsEmailConfirmed(LoginAccountDto loginInfo);
	}
}
