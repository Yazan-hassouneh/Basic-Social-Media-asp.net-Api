﻿using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class PostRepository(ApplicationDbContext context) : BaseRepository<Post>(context), IPostRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<string?> GetUserId(int postId)
		{
			Post? post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(post => post.Id == postId);
			return post?.UserId;
		}
		public override async Task<Post?> GetByIdAsync(int id)
		{
			Post? post = await _context.Posts
				.Include(post => post.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(post => post.Files)
				.Where(post => !post.User!.IsDeleted)
				.Select(post => new Post // Or ChatViewModel
				{
					Id = post.Id,
					CreatedOn = post.CreatedOn,
					Audience = post.Audience,
					Content = post.Content,
					Files = post.Files.Select(file => new PostFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						PostId = file.PostId,
						Path = file.Path,
					}).ToList(),
					IsDeleted = post.IsDeleted,
					UserId = post.UserId,
					RowVersion = post.RowVersion,
					User = post.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = post.User.Id,
						UserName = post.User.UserName,
						ProfileImageModel = post.User.ProfileImageModel
					},
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return post;
		}
		public async Task<IEnumerable<Post?>> GetAllAsync(string userId)
		{
			return await _context.Posts
				.Where(post => post.UserId == userId)
				.Where(post => !post.User!.IsDeleted)
				.Include(post => post.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(post => post.Files)
				.Select(post => new Post
				{
					Id = post.Id,
					CreatedOn = post.CreatedOn,
					Audience = post.Audience,
					Content = post.Content,
					Files = post.Files.Select(file => new PostFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						Path = file.Path,
					}).ToList(),
					IsDeleted = post.IsDeleted,
					UserId= post.UserId,
					User = post.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = post.User.Id,
						UserName = post.User.UserName,
						ProfileImageModel = post.User.ProfileImageModel
					},
				})
				.AsNoTracking()
				.AsSplitQuery() // Use split query for better performance with large data sets
				.ToListAsync();
		}		
		public async Task<IEnumerable<Post?>> GetAllAsync(Expression<Func<Post, bool>> matcher)
		{
			return await _context.Posts
				.Where(matcher)
				.Where(post => !post.User!.IsDeleted)
				.Include(post => post.User)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(post => post.Files)
				.Select(post => new Post
				{
					Id = post.Id,
					CreatedOn = post.CreatedOn,
					Audience = post.Audience,
					Content = post.Content,
					Files = post.Files.Select(file => new PostFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						Path = file.Path,
					}).ToList(),
					IsDeleted = post.IsDeleted,
					UserId = post.UserId,
					User = post.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = post.User.Id,
						UserName = post.User.UserName,
						ProfileImageModel = post.User.ProfileImageModel
					},
				})
				.AsNoTracking()
				.AsSplitQuery() // Use split query for better performance with large data sets
				.ToListAsync();
		}
	}
}
