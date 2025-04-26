using BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.MessagingRepos
{
	internal class MessagesRepository(ApplicationDbContext context) : BaseRepository<Message>(context), IMessageRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<string?> GetUserId(int messageId)
		{
			Message? message = await _context.Messages.AsNoTracking().FirstOrDefaultAsync(message => message.Id == messageId);
			return "Empty String";
		}
		public async Task<Message?> MessageExist(int messageId)
		{
			Message? message = await _context.Messages
				.Select(message => new Message
				{
					Id = message.Id,
					SenderId = message.SenderId,
					ReceiverId = message.ReceiverId,
					ChatId = message.ChatId,
				})
				.FirstOrDefaultAsync(message => message.Id == messageId);

			return message;
		}
		public async Task<Message?> GetByIdAsync(int id, string userId)
		{
			Message? message = await _context.Messages
				.Where(message => !message.DeletedByUsers.Any(deletedMessage => deletedMessage.UserId == userId && deletedMessage.MessageId == id))
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
					SenderId = message.SenderId,
					ReceiverId = message.ReceiverId,
					IsRead = message.IsRead,
					ChatId = message.ChatId,
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(message => message.Id == id);

			return message;
		}
		public async Task<IEnumerable<Message?>> GetAllAsync(int chatId, string userId)
		{
			var chatDeletedAt = await _context.ChatDeletions
				.Where(cd => cd.ChatId == chatId && cd.UserId == userId)
				.Select(cd => (DateTime?)cd.DeletedAt)
				.FirstOrDefaultAsync();

			var messages = await _context.Messages
				.Where(m => m.ChatId == chatId && (chatDeletedAt == null || m.CreatedOn > chatDeletedAt))
				.Include(m => m.Files)
				.Include(m => m.DeletedByUsers)
				.AsNoTracking()
				.ToListAsync();

			return messages.Select(m =>
			{
				var isMessageDeleted = m.DeletedByUsers.Any(d => d.UserId == userId);

				return new Message
				{
					Id = m.Id,
					CreatedOn = m.CreatedOn,
					Content = isMessageDeleted ? string.Empty : m.Content,
					Files = isMessageDeleted ? [] : m.Files.Select(f => new MessageFileModel
					{
						Id = f.Id,
						UserId = f.UserId,
						Path = f.Path,
					}).ToList(),
					IsDeleted = isMessageDeleted,
					SenderId = m.SenderId,
					ReceiverId = m.ReceiverId,
					IsRead = m.IsRead,
					ChatId = m.ChatId,
				};
			}).OrderByDescending(m => m.CreatedOn);
		}

	}
}
