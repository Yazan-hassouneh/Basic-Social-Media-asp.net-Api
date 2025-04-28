using AutoMapper;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class ChatService(IUnitOfWork unitOfWork, IMapper mapper, IMessagesServices messagesServices, IMessageFileModelService messageFileModelService) : IChatServices
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly IMessagesServices _messagesServices = messagesServices;
		private readonly IMessageFileModelService _messageFileModelService = messageFileModelService;

		public async Task<Dictionary<string, string>?> GetUsersId(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat == null) return null;
			Dictionary<string, string> users = new()
		   {
			   { "user1Id", chat.User1Id },
			   { "user2Id", chat.User2Id }
		   };
			return users;
		}
		public async Task<IEnumerable<string>?> GetFilesByChatIdAsync(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null) return null;
			return await _messageFileModelService.GetAllFilesByChatIdAsync(chatId);
		}
		public async Task<GetChatDto?> GetChatByIdAsync(int chatId, string userId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null) return null;
			GetChatDto chatDto = _mapper.Map<GetChatDto>(chat);
			chatDto.Messages = await _messagesServices.GetMessagesByChatIdAsync(chatId, userId);
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
				User1Id = chat.SenderId,
				User2Id = chat.ReceiverId
			};

			await _unitOfWork.Chats.AddAsync(newChat);
			await _unitOfWork.Chats.Save();

			return chat;
		}
		public async Task<bool> DeleteChatAsync(int chatId, string userId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null) return false;

			var existing = await _unitOfWork.ChatDeletion.FindWithTrackingAsync(cd => cd.ChatId == chatId && cd.UserId == userId);

			if (existing == null)
			{
				await _unitOfWork.ChatDeletion.AddAsync(new ChatDeletion
				{
					ChatId = chatId,
					UserId = userId,
					DeletedAt = DateTime.UtcNow
				});
			}
			else
			{
				existing.DeletedAt = DateTime.UtcNow;
				_unitOfWork.ChatDeletion.Update(existing);
			}

			await _unitOfWork.ChatDeletion.Save();

			/*
				Add Background Job to check if both users deleted the chat, if so delete the chat and deletedChat  

				_unitOfWork.Chats.Delete(chat);
				await _unitOfWork.Chats.Save();
			 */
			return true;
		}

	}
}
