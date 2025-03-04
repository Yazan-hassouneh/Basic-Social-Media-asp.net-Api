using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class MessagesRepository(ApplicationDbContext context) : BaseRepository<Message>(context), IMessageRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Message?> GetByIdAsync(int id)
		{
			Message? message = await _context.Messages
				.AsNoTracking()
				.FirstOrDefaultAsync(message => message.Id == id);

			return message;
		}
		public async Task<IEnumerable<Message?>> GetAllAsync(int chatId)
		{
			return await _context.Messages
				.Where(m => m.ChatId == chatId)
				.OrderBy(m => m.CreatedOn)
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
