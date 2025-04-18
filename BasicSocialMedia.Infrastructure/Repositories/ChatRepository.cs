﻿using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class ChatRepository(ApplicationDbContext context) : BaseRepository<Chat>(context), IChatRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Chat?> GetByIdAsync(int id)
		{
			Chat? chat = await _context.Chats
				.Include(chat => chat.User1)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(chat => chat.User2)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(chat => chat.Files)
				.Select(chat => new Chat
				{
					Id = chat.Id,
					Files = chat.Files.Select(file => new MessageFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						Path = file.Path,
					}).ToList(),
					CreatedOn = chat.CreatedOn,
					User1 = chat.User1 == null ? null : new ApplicationUser
					{
						Id = chat.User1.Id,
						UserName = chat.User1.UserName,
						ProfileImageModel = chat.User1.ProfileImageModel
					},
					User2 = chat.User2 == null ? null : new ApplicationUser
					{
						Id = chat.User2.Id,
						UserName = chat.User2.UserName,
						ProfileImageModel = chat.User2.ProfileImageModel
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return chat;
		}
		public async Task<IEnumerable<Chat?>> GetAllAsync(string userId)
		{
			return await _context.Chats
				.Where(chat => chat.User1Id == userId || chat.User2Id == userId)
				.Include(chat => chat.User1)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(chat => chat.User2)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(chat => new Chat
				{
					Id = chat.Id,
					CreatedOn = chat.CreatedOn,
					User1 = chat.User1 == null ? null : new ApplicationUser
					{
						Id = chat.User1.Id,
						UserName = chat.User1.UserName,
						ProfileImageModel = chat.User1.ProfileImageModel
					},
					User2 = chat.User2 == null ? null : new ApplicationUser
					{
						Id = chat.User2.Id,
						UserName = chat.User2.UserName,
						ProfileImageModel = chat.User2.ProfileImageModel
					}
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
