using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class AccountService(UserManager<ApplicationUser> userManager, IJWTService jWTService) : IAccountService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJWTService _jWTService = jWTService;

		public async Task<AuthDto> RegisterAsync(CreateAccountDto model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null) return new AuthDto { Message = "Email Already Exist" };
			if (await _userManager.FindByNameAsync(model.UserName) is not null) return new AuthDto { Message = "UserName Already Exist" };

			var newUser = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
			};

			var result = await _userManager.CreateAsync(newUser, model.Password);

			if (!result.Succeeded)
			{
				string errors = string.Empty;
				foreach (var error in result.Errors) errors += $"{error.Description},";
				return new AuthDto { Message = errors };
			}
			await _userManager.AddToRoleAsync(newUser, "User");

			// Add Claims
			var claims = new List<Claim>
			{
				new (ClaimTypes.NameIdentifier, newUser.Id),
				//new (ClaimTypes.Name, newUser.UserName),
				//new (ClaimTypes.Email, newUser.Email),
				//new (ClaimTypes.Role, ProjectEnums.AllowedRoles.User.ToString()) 
			};
			await _userManager.AddClaimsAsync(newUser, claims);

			var jwtSecurityToken = await _jWTService.CreateJwtToken(newUser);
			var refreshToken = _jWTService.GenerateRefreshToken();

			return new AuthDto
			{
				Email = newUser.Email,
				UserName = newUser.UserName,
				//ExpiresOn = jwtSecurityToken.ValidTo,
				RefreshToken = refreshToken.Token,
				Message = string.Empty,
				IsAuthenticated = true,
				UserRoles = ["User"],
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
			};
		}
	}
}
