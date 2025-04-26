using AutoMapper;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.Messaging;
using Ganss.Xss;
using Microsoft.Extensions.Logging;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class MessageService(IUnitOfWork unitOfWork, IMapper mapper, IMessageFileModelService messageFileModelService, ILogger<MessageService> logger, HtmlSanitizer sanitizer) : IMessagesServices
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly IMessageFileModelService _messageFileModelService = messageFileModelService;
		private readonly ILogger<MessageService> _logger = logger;
		private readonly HtmlSanitizer _sanitizer = sanitizer;

		public Task<string?> GetUserId(int messageId)
		{
			throw new NotImplementedException();
		}
		public async Task<MessageExistDto?> GetMessageByIdAsync(int messageId)
		{
			Message? existingMessage = await _unitOfWork.Messages.MessageExist(messageId);

			if (existingMessage == null) return null;
			return _mapper.Map<MessageExistDto?>(existingMessage);

		}
		public async Task<IEnumerable<GetMessagesDto>> GetMessagesByChatIdAsync(int chatId, string userId)
		{
			IEnumerable<Message?> messages = await _unitOfWork.Messages.GetAllAsync(chatId, userId);
			IEnumerable<Message?> noneNullMessages = messages.Where(message => message != null);

			if (noneNullMessages == null || !noneNullMessages.Any()) return Enumerable.Empty<GetMessagesDto>();

			return _mapper.Map<IEnumerable<GetMessagesDto>>(noneNullMessages);	
		}
		public async Task<bool> CreateMessageAsync(AddMessageDto message)
		{
			Message newMessage = _mapper.Map<Message>(message);
			try
			{
				var addedMessage = await _unitOfWork.Messages.AddAsync(newMessage);
				int affectedRows = await _unitOfWork.Messages.Save();

				if (message.Files != null && message.Files.Count > 0 && affectedRows > 0)
				{
					AddMessageFileDto addMessageFileDto = _mapper.Map<AddMessageFileDto>(message);
					addMessageFileDto.MessageId = addedMessage.Id;

					bool fileResult = await _messageFileModelService.AddMessageFileAsync(addMessageFileDto);
					return fileResult;
				}

				return affectedRows > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a message");
				return false;
			}
		}
		public async Task<bool> UpdateMessageAsync(UpdateMessageDto messageDto)
		{
			if (messageDto is null) return false;
			CancellationToken cancellationToken = new();
			bool isMessageExist = await _unitOfWork.Messages.DoesExist(messageDto.Id, cancellationToken);
			if (!isMessageExist) return false;

			Message? message = await _unitOfWork.Messages.GetByIdWithTrackingAsync(messageDto.Id);
			if (message == null) return false;

			bool hasNewFiles = messageDto.Files is not null && messageDto.Files.Count > 0;
			List<string>? oldMediaPath = messageDto.MediaPaths ?? [];
			IEnumerable<MessageFileModel?> messageFileModels = await _unitOfWork.MessageFiles.FindAllAsync(file => file.MessageId == messageDto.Id);

			UpdateMessageFileDto? updateMessageFileDto = null;
			if (hasNewFiles || oldMediaPath!.Count > 0)
			{
				updateMessageFileDto = new UpdateMessageFileDto()
				{
					UserId = messageDto!.UserId,
					MessageId = message.Id,
					Files = messageDto.Files,
					MediaPaths = oldMediaPath
				};
			}
			bool isContentChanged = message.Content != messageDto?.Content;
			if (isContentChanged)
			{
				message.Content = _sanitizer.Sanitize(messageDto?.Content ?? string.Empty);
				_unitOfWork.Messages.Update(message);
			}
			int effectedRows = await _unitOfWork.Messages.Save();

			if (effectedRows > 0 || !isContentChanged)
			{
				if (updateMessageFileDto != null)
				{
					updateMessageFileDto.ChatId = message.ChatId;
					await _messageFileModelService.UpdateMessageFileAsync(updateMessageFileDto);
				}
				if (updateMessageFileDto is null && messageFileModels.Any())
				{
					await _messageFileModelService.DeleteMessageFileByMessageIdAsync(messageDto!.Id);
				}
			}
			return true;
		}
		public async Task<bool> DeleteMessageAsync(int messageId)
		{
			Message? existingMessage = await _unitOfWork.Messages.GetByIdAsync(messageId);

			if (existingMessage == null) return false;

			DeletedMessage deletedMessage = _mapper.Map<DeletedMessage>(existingMessage);
			await _unitOfWork.DeletedMessages.AddAsync(deletedMessage);
			await _unitOfWork.DeletedMessages.Save();

			/*
					Add Background Job To check if the both user deleted the message, if true 
					delete message from the database, and remove the deletedMessage from the database .
			 */

			return true;
		}


	}
}
