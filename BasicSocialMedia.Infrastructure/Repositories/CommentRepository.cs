using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment>(context), ICommentRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Comment?> GetByIdAsync(int id)
		{
			Comment? comment = await _context.Comments
				.Include(comment => comment.User)
				.Include(comment => comment.CommentReactions)
					.ThenInclude(reaction => reaction.User)
				.Select(comment => new Comment
				{
					Id = comment.Id,
					CreatedOn = comment.CreatedOn,
					Content = comment.Content,
					RowVersion = comment.RowVersion,
					CommentReactions = comment.CommentReactions.Select(cr => new CommentReaction
					{
						Id = cr.Id,
						UserId = cr.UserId,
						User = cr.User == null ? null : new ApplicationUser
						{
							Id = cr.User.Id,
							UserName = cr.User.UserName,
							ProfileImage = cr.User.ProfileImage,
						},
					}).ToList(),
					// ... other properties of comment ...
					User = comment.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = comment.User.Id,
						UserName = comment.User.UserName,
						ProfileImage = comment.User.ProfileImage
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return comment;
		}
		public async Task<IEnumerable<Comment?>> GetAllAsync(int postId)
		{
			return await _context.Comments
				.Where(c => c.PostId == postId)
				.Include(comment => comment.User)
				.Include(comment => comment.CommentReactions)
					.ThenInclude(reaction => reaction.User)
				.Select(comment => new Comment
				{
					Id = comment.Id,
					CreatedOn = comment.CreatedOn,
					Content = comment.Content,
					RowVersion = comment.RowVersion,
					CommentReactions = comment.CommentReactions.Select(cr => new CommentReaction
					{
						Id = cr.Id,
						UserId = cr.UserId,
						User = cr.User == null ? null : new ApplicationUser
						{
							Id = cr.User.Id,
							UserName = cr.User.UserName,
							ProfileImage = cr.User.ProfileImage,
						},
					}).ToList(),
					// ... other properties of comment ...
					User = comment.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = comment.User.Id,
						UserName = comment.User.UserName,
						ProfileImage = comment.User.ProfileImage
					}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
