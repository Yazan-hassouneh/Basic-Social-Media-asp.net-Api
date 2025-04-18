using AutoMapper;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class ChatService(IUnitOfWork unitOfWork, IMapper mapper, IMessagesServices messagesServices) : IChatServices
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly IMessagesServices _messagesServices = messagesServices;
		public async Task<GetChatDto?> GetChatByIdAsync(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null) return null;
			GetChatDto chatDto = _mapper.Map<GetChatDto>(chat);
			IEnumerable<GetMessagesDto> messages = await _messagesServices.GetMessagesByChatIdAsync(chatId);
			chatDto.Messages = messages;

			return chatDto;
		}
		public async Task<IEnumerable<GetChatDto>?> GetChatsByUserIdAsync(string userId)
		{
			IEnumerable<Chat?> chats = await _unitOfWork.Chats.GetAllAsync(userId);

			if (chats is null || !chats.Any()) return Enumerable.Empty<GetChatDto>();

			List<GetChatDto> chatDtos = chats
				.Where(chat => chat is not null)
				.Select(chat => _mapper.Map<GetChatDto>(chat!))
				.ToList();

			return chatDtos;
		}
		public async Task<AddChatDto> CreateChatAsync(AddChatDto chat)
		{
			Chat newChat = new()
			{
				User1Id = chat.User1Id,
				User2Id = chat.User2Id
			};

			await _unitOfWork.Chats.AddAsync(newChat);
			await _unitOfWork.Chats.Save();

			return chat;
		}
		public async Task<bool> DeleteChatAsync(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null) return false;

			_unitOfWork.Chats.Delete(chat);
			await _unitOfWork.Chats.Save();
			return true;
		}

		public Task<string?> GetUserId(int chatId)
		{
			throw new NotImplementedException();
		}
	}
}
