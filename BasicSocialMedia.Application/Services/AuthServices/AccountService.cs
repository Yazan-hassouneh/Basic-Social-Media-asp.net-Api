using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class AccountService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, IJWTService jWTService, IUserBackgroundJobsServices userBackgroundJobsServices) : IAccountService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJWTService _jWTService = jWTService;
		private readonly IUserBackgroundJobsServices _userBackgroundJobsServices = userBackgroundJobsServices;
		private readonly IEmailSender _emailSender = emailSender;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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

			// Send email confirmation
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
			var encodedToken = WebUtility.UrlEncode(token);
			var request = _httpContextAccessor.HttpContext?.Request;
			if (request == null) return new AuthDto { Message = "Request is null" };
			var confirmationUrl = $"{request.Scheme}://{request.Host}/api/Account/confirm-email?userId={newUser.Id}&token={encodedToken}";
			await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", $"Please confirm your account by clicking <a href='{confirmationUrl}'>here</a>.");

			// Add to default role
			await _userManager.AddToRoleAsync(newUser, "User");

			// Add Claims  
			var claims = new List<Claim>
			{
			   new (ClaimTypes.NameIdentifier, newUser.Id),
			};
			await _userManager.AddClaimsAsync(newUser, claims);

			var jwtSecurityToken = await _jWTService.CreateJwtToken(newUser);
			var refreshToken = _jWTService.GenerateRefreshToken();

			return new AuthDto
			{
				Email = newUser.Email,
				UserName = newUser.UserName,
				RefreshToken = refreshToken.Token,
				Message = string.Empty,
				IsAuthenticated = true,
				UserRoles = ["User"],
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
			};
		}
		public async Task<bool> TryCancelScheduledDeletionAsync(LoginAccountDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return false;

			if (user.IsDeleted)
			{
				string? jobId = await _userBackgroundJobsServices.RetrieveJobIdAsync(user.Id, BackgroundJobTypes.DeleteUserAccount.ToString());
				if (string.IsNullOrEmpty(jobId)) return false;
				await _userBackgroundJobsServices.DeleteJobIdAsync(user.Id, BackgroundJobTypes.DeleteUserAccount.ToString());
				user.IsDeleted = false;
				await _userManager.UpdateAsync(user);
				return true;
			}

			return false;
		}
		public async Task<bool> IsEmailConfirmed(LoginAccountDto loginInfo)
		{
			var user = await _userManager.FindByEmailAsync(loginInfo.Email);
			if (user == null) return false;
			return await _userManager.IsEmailConfirmedAsync(user);
		}
	}
}
