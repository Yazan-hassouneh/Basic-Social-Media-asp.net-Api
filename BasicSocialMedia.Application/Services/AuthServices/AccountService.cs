using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

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

			var jwtSecurityToken = await _jWTService.CreateJwtToken(newUser);
			var refreshToken = _jWTService.GenerateRefreshToken();

			return new AuthDto
			{
				UserEmail = newUser.Email,
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
