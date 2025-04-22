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

		public async Task<string?> GetUserId(int commentReactionId)
		{
			PostReaction? commentReaction = await _context.PostReactions.AsNoTracking().FirstOrDefaultAsync(commentReaction => commentReaction.Id == commentReactionId);
			return commentReaction?.UserId;
		}
		public override async Task<PostReaction?> GetByIdAsync(int id)
		{
			PostReaction? postReaction = await _context.PostReactions
				.Include(postReaction => postReaction.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(postReaction => new PostReaction 
				{
					Id = postReaction.Id,
					CreatedOn = postReaction.CreatedOn,
					User = postReaction.User == null
						? null
						: postReaction.User.IsDeleted
							? new ApplicationUser // Or anonymous type, handle potential nulls
							{
								Id = postReaction.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = postReaction.User.Id,
								UserName = postReaction.User.UserName,
								ProfileImageModel = postReaction.User.ProfileImageModel
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
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(postReaction => new PostReaction
				{
					Id = postReaction.Id,
					CreatedOn = postReaction.CreatedOn,
					User = postReaction.User == null
						? null
						: postReaction.User.IsDeleted
							? new ApplicationUser // Or anonymous type, handle potential nulls
							{
								Id = postReaction.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = postReaction.User.Id,
								UserName = postReaction.User.UserName,
								ProfileImageModel = postReaction.User.ProfileImageModel
							}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
