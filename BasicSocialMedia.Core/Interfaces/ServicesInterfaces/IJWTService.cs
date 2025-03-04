using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using System.IdentityModel.Tokens.Jwt;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces
{
	public interface IJWTService
	{
		Task<AuthDto> GetTokenAsync(LoginAccountDto model);
		Task<AuthDto> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
		Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
	}
}
