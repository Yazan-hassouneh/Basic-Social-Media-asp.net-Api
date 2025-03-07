using BasicSocialMedia.Application.Helpers;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class JWTService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt) : IJWTService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly JWT _jwt = jwt.Value;

		public async Task<AuthDto> GetTokenAsync(LoginAccountDto model)
		{
			var authModel = new AuthDto();
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "Email or Password is incorrect!";
				return authModel;
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.UserEmail = user.Email;
			authModel.UserName = user.UserName;
			//authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.UserRoles = [.. rolesList];

			if (user.RefreshTokens.Any(token => token.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(token => token.IsActive);
				authModel.RefreshToken = activeRefreshToken.Token;
				authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var refreshToken = GenerateRefreshToken();
				authModel.RefreshToken = refreshToken.Token;
				authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}
			return authModel;
		}
		public async Task<AuthDto> RefreshTokenAsync(string token)
		{
			AuthDto authModel = new();
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				authModel.IsAuthenticated = false;
				authModel.Message = "Invalid Token";
				return authModel;
			}

			var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

			if (refreshToken == null || !refreshToken.IsActive)
			{
				authModel.IsAuthenticated = false;
				authModel.Message = "Inactive Token";
				return authModel;
			}

			refreshToken.RevokedOn = DateTime.UtcNow;
			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);

			var jwtToken = await CreateJwtToken(user);
			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			authModel.UserEmail = user.Email;
			authModel.UserName = user.UserName;

			var roles = await _userManager.GetRolesAsync(user);
			authModel.UserRoles = [.. roles];
			authModel.RefreshToken = newRefreshToken.Token;
			authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

			return authModel;
		}
		public async Task<bool> RevokeTokenAsync(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token)); 
			if (user == null) return false;
			var refreshToken = user.RefreshTokens?.SingleOrDefault(t => t.Token == token);
			if (refreshToken == null || !refreshToken.IsActive) return false;
			refreshToken.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);
			return true;
		}
		public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("userId", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}
		public RefreshToken GenerateRefreshToken()
		{
			byte[] randomNumber = new byte[32];
			RandomNumberGenerator.Fill(randomNumber);

			return new RefreshToken
			{
				//Base64 strings can include characters like + and /, which might cause issues in certain storage formats, 
				// This makes it URL-safe and avoids padding characters (=).
				Token = Convert.ToBase64String(randomNumber).Replace("+", "-").Replace("/", "_").TrimEnd('='),
				ExpiresOn = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
				CreatedOn = DateTime.UtcNow
			};
		}
	}
}
