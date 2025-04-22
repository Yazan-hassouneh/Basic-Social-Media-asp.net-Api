using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment>(context), ICommentRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<string?> GetUserId(int commentId)
		{
			Comment? comment = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(comment => comment.Id == commentId);
			return comment?.UserId;
		}
		public override async Task<Comment?> GetByIdAsync(int id)
		{
			Comment? comment = await _context.Comments
				.Include(comment => comment.User)
				.Include(comment => comment.CommentReactions)
					.ThenInclude(reaction => reaction.User)
						.ThenInclude(user => user!.ProfileImageModel)
				.Include(comment => comment.Files)
				.Select(comment => new Comment
				{
					Id = comment.Id,
					CreatedOn = comment.CreatedOn,
					Content = comment.Content,
					PostId = comment.PostId,
					RowVersion = comment.RowVersion,
					CommentReactions = comment.CommentReactions.Select(cr => new CommentReaction
					{
						Id = cr.Id,
						UserId = cr.UserId,
						User = cr.User == null 
						? null 
						: cr.User!.IsDeleted 
							? new ApplicationUser
							{
								Id = cr.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = cr.User.Id,
								UserName = cr.User.UserName,
								ProfileImageModel = cr.User.ProfileImageModel,
							},
					}).ToList(),
					Files = comment.Files.Select(file => new CommentFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						PostId = file.PostId,
						CommentId = comment.Id,
						Path = file.Path,
					}).ToList(),
					// ... other properties of comment ...
					User = comment.User == null 
					? null 
					: comment.User.IsDeleted 
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = comment.User.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = comment.User.Id,
							UserName = comment.User.UserName,
							ProfileImageModel = comment.User.ProfileImageModel
						}
				})
				.AsNoTracking()
				.AsSplitQuery() // Use split query for better performance with large data sets
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
						.ThenInclude(user => user!.ProfileImageModel)
				.Include(comment => comment.Files)
				.Select(comment => new Comment
				{
					Id = comment.Id,
					CreatedOn = comment.CreatedOn,
					Content = comment.Content,
					PostId = comment.PostId,
					RowVersion = comment.RowVersion,
					CommentReactions = comment.CommentReactions.Select(cr => new CommentReaction
					{
						Id = cr.Id,
						UserId = cr.UserId,
						User = cr.User == null 
						? null 
						: cr.User!.IsDeleted 
							? new ApplicationUser
							{
								Id = cr.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = cr.User.Id,
								UserName = cr.User.UserName,
								ProfileImageModel = cr.User.ProfileImageModel,
							},
					}).ToList(),
					Files = comment.Files.Select(file => new CommentFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						PostId = file.PostId,
						CommentId = comment.Id,
						Path = file.Path,
					}).ToList(),
					// ... other properties of comment ...
					User = comment.User == null
					? null
					: comment.User.IsDeleted
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = comment.User.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = comment.User.Id,
							UserName = comment.User.UserName,
							ProfileImageModel = comment.User.ProfileImageModel
						}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
