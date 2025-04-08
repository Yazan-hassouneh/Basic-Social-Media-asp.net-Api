using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.FileModels;
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
				.Include(message => message.Files)
				.Select(message => new Message
				{
					Id = message.Id,
					CreatedOn = message.CreatedOn,
					Content = message.Content,
					Files = message.Files.Select(file => new MessageFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						Path = file.Path,
					}).ToList(),
					User1Id = message.User1Id,
					User2Id = message.User2Id,
					IsRead = message.IsRead,
					ChatId = message.ChatId,
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(message => message.Id == id);

			return message;
		}
		public async Task<IEnumerable<Message?>> GetAllAsync(int chatId)
		{
			return await _context.Messages
				.Where(m => m.ChatId == chatId)
				.Include(message => message.Files)
				.Select(message => new Message
				{
					Id = message.Id,
					CreatedOn = message.CreatedOn,
					Content = message.Content,
					Files = message.Files.Select(file => new MessageFileModel
					{
						Id = file.Id,
						UserId = file.UserId,
						Path = file.Path,
					}).ToList(),
					User1Id = message.User1Id,
					User2Id = message.User2Id,
					IsRead = message.IsRead,
					ChatId = message.ChatId,
				})
				.OrderBy(m => m.CreatedOn)
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
