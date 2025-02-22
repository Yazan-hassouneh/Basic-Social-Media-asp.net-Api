using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
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
				.Include(chat => chat.User2)
				.Select(chat => new Chat 
					{
						Id = chat.Id,
						CreatedOn = chat.CreatedOn,
						// ... other properties of Chat ...
						User1 = chat.User1 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User1.Id,
							UserName = chat.User1.UserName,
							ProfileImage = chat.User1.ProfileImage
						},
						User2 = chat.User2 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User2.Id,
							UserName = chat.User2.UserName,
							ProfileImage = chat.User2.ProfileImage
						}
					})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return chat;
		}
		public override async Task<IEnumerable<Chat?>> GetAllAsync()
		{
			return await _context.Chats
				.Include(chat => chat.User1)
				.Include(chat => chat.User2)
				.Select(chat => new Chat // Or ChatViewModel
					{
						Id = chat.Id,
						CreatedOn = chat.CreatedOn,
						// ... other properties of Chat ...
						User1 = chat.User1 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User1.Id, 
							UserName = chat.User1.UserName,
							ProfileImage = chat.User1.ProfileImage
						},
						User2 = chat.User2 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
						{
							Id = chat.User2.Id, 
							UserName = chat.User2.UserName,
							ProfileImage = chat.User2.ProfileImage
						}
					})
				.AsNoTracking()
				.ToListAsync(); 
		}
	}
}
