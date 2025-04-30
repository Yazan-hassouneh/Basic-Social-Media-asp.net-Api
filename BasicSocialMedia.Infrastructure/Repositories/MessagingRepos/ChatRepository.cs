using BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.MessagingRepos
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
				.Select(chat => new Chat
				{
					Id = chat.Id,
					CreatedOn = chat.CreatedOn,
					User1Id = chat.User1Id,
					User2Id	= chat.User2Id,
					User1 = chat.User1 == null
					? null
					: chat.User1.IsDeleted
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User1.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = chat.User1.Id,
							UserName = chat.User1.UserName,
							ProfileImageModel = chat.User1.ProfileImageModel
						},
					User2 = chat.User2 == null
					? null
					: chat.User2.IsDeleted
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User2.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = chat.User2.Id,
							UserName = chat.User2.UserName,
							ProfileImageModel = chat.User2.ProfileImageModel
						},
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
					User1 = chat.User1 == null
					? null
					: chat.User1.IsDeleted
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User1.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = chat.User1.Id,
							UserName = chat.User1.UserName,
							ProfileImageModel = chat.User1.ProfileImageModel
						},
					User2 = chat.User2 == null
					? null
					: chat.User2.IsDeleted
						? new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User2.Id,
							UserName = "Deleted User",
							ProfileImageModel = null
						}
						: new ApplicationUser
						{
							Id = chat.User2.Id,
							UserName = chat.User2.UserName,
							ProfileImageModel = chat.User2.ProfileImageModel
						},
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
