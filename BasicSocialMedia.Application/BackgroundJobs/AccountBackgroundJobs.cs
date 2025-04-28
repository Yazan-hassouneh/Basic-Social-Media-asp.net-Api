using BasicSocialMedia.Core.Interfaces.BackgroundJobsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using Hangfire;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.BackgroundJobs
{
	public class AccountBackgroundJobs(UserManager<ApplicationUser> userManager) : IAccountBackgroundJobs
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;

		[AutomaticRetry(Attempts = 0)]
		public async Task<IdentityResult> HardDeleteUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new InvalidOperationException($"User with ID {userId} was not found.");
			}

			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				// Optionally log or throw depending on your style
				throw new InvalidOperationException($"Failed to delete user with ID {userId}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
			}

			return result;
		}
	}
}
