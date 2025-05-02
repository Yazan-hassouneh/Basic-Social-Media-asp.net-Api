using BasicSocialMedia.Core.Interfaces.Repos.Auth;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.Auth
{
	internal class UserBackgroundJobsRepository(ApplicationDbContext context) : BaseRepository<UserBackgroundJob>(context), IUserBackgroundJobs
	{
	}
}
