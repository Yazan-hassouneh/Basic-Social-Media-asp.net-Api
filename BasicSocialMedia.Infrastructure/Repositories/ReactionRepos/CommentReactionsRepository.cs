﻿using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;
using Superpower.Parsers;

namespace BasicSocialMedia.Infrastructure.Repositories.ReactionRepos
{
	internal class CommentReactionsRepository(ApplicationDbContext context) : BaseRepository<CommentReaction>(context), ICommentReactionRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<string?> GetUserId(int commentReactionId)
		{
			CommentReaction? commentReaction = await _context.CommentReactions.AsNoTracking().FirstOrDefaultAsync(commentReaction => commentReaction.Id == commentReactionId);
			return commentReaction?.UserId;
		}
		public override async Task<CommentReaction?> GetByIdAsync(int id)
		{
			CommentReaction? commentReaction = await _context.CommentReactions
				.Include(commentReaction => commentReaction.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(commentReaction => new CommentReaction
				{
					Id = commentReaction.Id,
					CreatedOn = commentReaction.CreatedOn,
					User = commentReaction.User == null
						? null
						: commentReaction.User.IsDeleted
							? new ApplicationUser // Or anonymous type, handle potential nulls
							{
								Id = commentReaction.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = commentReaction.User.Id,
								UserName = commentReaction.User.UserName,
								ProfileImageModel = commentReaction.User.ProfileImageModel
							}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return commentReaction;
		}
		public async Task<IEnumerable<CommentReaction?>> GetAllAsync(int commentId)
		{
			return await _context.CommentReactions
				.Where(commentReaction => commentReaction.CommentId == commentId)
				.Include(commentReaction => commentReaction.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(commentReaction => new CommentReaction
				{
					Id = commentReaction.Id,
					CreatedOn = commentReaction.CreatedOn,
					User = commentReaction.User == null
						? null
						: commentReaction.User.IsDeleted
							? new ApplicationUser // Or anonymous type, handle potential nulls
							{
								Id = commentReaction.User.Id,
								UserName = "Deleted User",
								ProfileImageModel = null
							}
							: new ApplicationUser
							{
								Id = commentReaction.User.Id,
								UserName = commentReaction.User.UserName,
								ProfileImageModel = commentReaction.User.ProfileImageModel
							}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
