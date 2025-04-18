using BasicSocialMedia.Core.Interfaces.Repos.FileModelsRepositories;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.FileRepos
{
	internal class ProfileImageModelRepository(ApplicationDbContext context) : BaseFileModelsRepository<ProfileImageModel>(context), IProfileImageModelRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<string?> GetUserId(int profileImageId)
		{
			ProfileImageModel? image = await _context.ProfileImages.AsNoTracking().FirstOrDefaultAsync(image => image.Id == profileImageId);
			return image?.UserId;
		}
	}
}
