using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class PostRepository(ApplicationDbContext context) : BaseRepository<Post>(context), IPostRepository
	{

	}
}
