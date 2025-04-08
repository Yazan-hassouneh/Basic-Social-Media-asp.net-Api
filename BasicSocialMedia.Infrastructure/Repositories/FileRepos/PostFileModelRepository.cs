using BasicSocialMedia.Core.Interfaces.Repos.FileModelsRepositories;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.FileRepos
{
	internal class PostFileModelRepository(ApplicationDbContext context) : BaseFileModelsRepository<PostFileModel>(context), IPostFileModelRepository
	{

	}
}
