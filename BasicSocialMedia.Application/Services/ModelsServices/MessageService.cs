using AutoMapper;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class MessageService(IUnitOfWork unitOfWork, IMapper mapper) : IMessagesServices
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		public async Task<IEnumerable<GetMessagesDto>> GetMessagesByChatIdAsync(int chatId)
		{
			IEnumerable<Message?> messages = await _unitOfWork.Messages.GetAllAsync(chatId);
			IEnumerable<Message?> noneNullMessages = messages.Where(message => message != null);

			if (noneNullMessages == null || !noneNullMessages.Any()) return Enumerable.Empty<GetMessagesDto>();

			return _mapper.Map<IEnumerable<GetMessagesDto>>(noneNullMessages);	
		}
		public async Task<AddMessageDto> CreateMessageAsync(AddMessageDto message)
		{
			var newMessage = new Message
			{
				Content = message.Content,
				User1Id = message.User1Id,
				User2Id = message.User2Id,
			};

			await _unitOfWork.Messages.AddAsync(newMessage);
			await _unitOfWork.Messages.Save();
			return message;
		}
		public async Task<UpdateMessageDto?> UpdateMessageAsync(UpdateMessageDto message)
		{
			Message? existingMessage = await _unitOfWork.Messages.GetByIdAsync(message.Id);

			if (existingMessage == null) return null;

			existingMessage.Content = message.Content;
			_unitOfWork.Messages.Update(existingMessage);
			await _unitOfWork.Messages.Save();
			return message;
		}
		public async Task<bool> DeleteMessageAsync(int messageId)
		{
			Message? existingMessage = await _unitOfWork.Messages.GetByIdAsync(messageId);

			if (existingMessage == null) return false;

			_unitOfWork.Messages.Delete(existingMessage);
			await _unitOfWork.Messages.Save();
			return true;
		}
	}
}
