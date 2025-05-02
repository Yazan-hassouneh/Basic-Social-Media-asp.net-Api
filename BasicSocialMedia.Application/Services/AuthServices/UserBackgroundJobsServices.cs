using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.AuthModels;
using Hangfire;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class UserBackgroundJobsServices(IUnitOfWork unitOfWork) : IUserBackgroundJobsServices
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;

		public async Task<bool> StoreBackgroundJobAsync(string jobId, string userId, string jobType)
		{
			if (string.IsNullOrEmpty(jobId) || string.IsNullOrEmpty(userId)) return false;

			UserBackgroundJob? existingJob = await _unitOfWork.UserBackgroundJobs.FindAsync(x => x.UserId == userId && x.JobType == jobType);
			if (existingJob is not null) return false;

			UserBackgroundJob userBackgroundJob = new()
			{
				JobId = jobId,
				UserId = userId,
				JobType = jobType
			};

			await _unitOfWork.UserBackgroundJobs.AddAsync(userBackgroundJob);
			await _unitOfWork.UserBackgroundJobs.Save();
			return true;
		}
		public async Task<string?> RetrieveJobIdAsync(string userId, string jobType)
		{
			UserBackgroundJob? userBackgroundJob = await _unitOfWork.UserBackgroundJobs.FindAsync(x => x.UserId == userId && x.JobType == jobType);
			return userBackgroundJob?.JobId;
		}
		public async Task<bool> DeleteJobIdAsync(string userId, string jobType)
		{
			var jobRecord = await _unitOfWork.UserBackgroundJobs.FindAsync(x => x.UserId == userId && x.JobType == jobType);
			if (jobRecord == null) return false;

			// Cancel the job in Hangfire
			bool deleted = BackgroundJob.Delete(jobRecord.JobId);

			// Optional: only delete from DB if job was found in Hangfire
			if (deleted)
			{
				_unitOfWork.UserBackgroundJobs.Delete(jobRecord);
				await _unitOfWork.UserBackgroundJobs.Save();
			}

			return deleted;
		}
	}
}
