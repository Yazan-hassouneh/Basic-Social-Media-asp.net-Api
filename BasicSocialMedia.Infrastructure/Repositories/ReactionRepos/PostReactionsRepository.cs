using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.ReactionRepos
{
	internal class PostReactionsRepository(ApplicationDbContext context) : BaseRepository<PostReaction>(context), IPostReactionRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<PostReaction?> GetByIdAsync(int id)
		{
			PostReaction? postReaction = await _context.PostReactions
				.Include(postReaction => postReaction.User)
				.Select(postReaction => new PostReaction 
				{
					Id = postReaction.Id,
					CreatedOn = postReaction.CreatedOn,
					User = postReaction.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = postReaction.User.Id,
						UserName = postReaction.User.UserName,
						ProfileImage = postReaction.User.ProfileImage
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return postReaction;
		}
		public async Task<IEnumerable<PostReaction?>> GetAllAsync(int postId)
		{
			return await _context.PostReactions
				.Where(postReaction => postReaction.PostId == postId)
				.Include(postReaction => postReaction.User)
				.Select(postReaction => new PostReaction
				{
					Id = postReaction.Id,
					CreatedOn = postReaction.CreatedOn,
					User = postReaction.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = postReaction.User.Id,
						UserName = postReaction.User.UserName,
						ProfileImage = postReaction.User.ProfileImage
					}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
