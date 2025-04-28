using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Core.Interfaces.BackgroundJobsInterfaces
{
	public interface IAccountBackgroundJobs 
	{
		Task<IdentityResult> HardDeleteUserAsync(string userId);
	}
}
