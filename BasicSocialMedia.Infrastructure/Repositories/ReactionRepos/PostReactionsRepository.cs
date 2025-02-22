using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.ReactionRepos
{
	internal class PostReactionsRepository(ApplicationDbContext context) : BaseRepository<PostReaction>(context), IPostReactionRepository
	{
	}
}
