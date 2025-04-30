using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using BasicSocialMedia.Application.BackgroundJobs;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IJWTService jwtService, IAccountService accountService, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IJWTService _jwtService = jwtService;
		private readonly IAccountService _accountService = accountService;
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IAuthorizationService _authorizationService = authorizationService;


		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create([FromBody] CreateAccountDto newAccount)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var result = await _accountService.RegisterAsync(newAccount);
			if (!result.IsAuthenticated) return BadRequest(result.Message);
			if (string.IsNullOrEmpty(result.RefreshToken)) return BadRequest("Something went wrong");
			SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
			return Ok(result);
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginAccountDto loginInfo)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var result = await _jwtService.GetTokenAsync(loginInfo);
			if (!result.IsAuthenticated) return BadRequest(result.Message);
			if (string.IsNullOrEmpty(result.RefreshToken)) return BadRequest("Something went wrong");
			SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
			return Ok(result);
		}

		[HttpGet("refreshToken")]
		[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
		public async Task<IActionResult> GetRefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh token is required");
			var result = await _jwtService.RefreshTokenAsync(refreshToken);
			if (!result.IsAuthenticated) return BadRequest(result);
			if (string.IsNullOrEmpty(result.RefreshToken)) return BadRequest("Something went wrong");
			SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);
			return Ok(result);
		}

		[HttpPost("revoke")]
		[Authorize(Policy = PoliciesSettings.allowSuperAdminAdminPolicy)]
		public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto model)
		{
			var token = model.Token ?? Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(token)) return BadRequest("Token is required");
			var result = await _jwtService.RevokeTokenAsync(token);
			if (!result) return BadRequest("Token is invalid");
			return Ok();
		}

		[HttpPut("delete/{userId}")]
		[Authorize]
		public async Task<IActionResult> SoftDeleteUser([FromRoute] string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDelete = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.allowSuperAdminAdminPolicy);
			if (!authorizationOwnership.Succeeded && !authorizationCanDelete.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null) return NotFound();
			if (user.IsDeleted) return BadRequest();

			user.IsDeleted = true;
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				BackgroundJob.Schedule<AccountBackgroundJobs>(x => x.HardDeleteUserAsync(userId), TimeSpan.FromSeconds(BackgroundJobsSettings.HardDeleteAccountAfterDays));
				return Ok("User Deleted Successfully");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return BadRequest(ModelState);
		}

		private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
		{
			var cookiesOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(),
			};
			Response.Cookies.Append("refreshToken", refreshToken, cookiesOptions);
		}
	}
}
