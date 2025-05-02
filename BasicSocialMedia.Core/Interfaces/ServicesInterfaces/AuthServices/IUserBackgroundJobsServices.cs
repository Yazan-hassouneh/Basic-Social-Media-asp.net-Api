namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IUserBackgroundJobsServices
	{
		Task<bool> StoreBackgroundJobAsync(string jobId, string userId, string jobType);
		Task<string?> RetrieveJobIdAsync(string userId, string jobType);
		Task<bool> DeleteJobIdAsync(string userId, string jobType);
	}
}
