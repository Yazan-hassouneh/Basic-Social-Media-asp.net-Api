using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.ReactionRepos
{
	internal class CommentReactionsRepository(ApplicationDbContext context) : BaseRepository<CommentReaction>(context), ICommentReactionRepository
	{
	}
}
