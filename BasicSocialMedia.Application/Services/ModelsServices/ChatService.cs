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
		public async Task<bool> SetUserIdToNull(string userId)
		{
			var chats = await _unitOfWork.Chats.FindAllWithTrackingAsync(chat => chat.User1Id == userId || chat.User2Id == userId);
			if (chats == null || !chats.Any()) return true;

			if (chats.Any())
			{
				foreach (var chat in chats)
				{
					bool updated = false;

					if (chat!.User1Id == userId)
					{
						chat.User1Id = null;
						updated = true;
					}

					if (chat.User2Id == userId)
					{
						chat.User2Id = null;
						updated = true;
					}

					if (updated)
					{
						_unitOfWork.Chats.Update(chat);
					}
				}

				await _unitOfWork.Chats.Save();
				return true;
			}
			return true;
		}
		public async Task<bool> SoftDeleteChatAsync(int chatId, string userId)
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

			if(await IsChatCompletelyDeletedAsync(chatId))
			{
				bool result =  await HardDeleteChatAsync(chatId);
				if (!result) return false;
			}
			return true;
		}
		public async Task<bool> IsChatCompletelyDeletedAsync(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
			if (chat is null || (chat.User1Id is null && chat.User2Id is null)) return true;
			var deletions = await _unitOfWork.ChatDeletion.FindAllAsync(cd => cd.ChatId == chatId);
			return deletions.Count() == 2;
		}		
		public async Task<bool> HardDeleteChatAsync(int chatId)
		{
			Chat? chat = await _unitOfWork.Chats.GetByIdWithTrackingAsync(chatId);
			if (chat is null) return false;

			// Delete associated file models  
			await _messageFileModelService.DeleteMessageFileByChatIdAsync(chatId);

			// Delete the chat itself  
			_unitOfWork.Chats.Delete(chat);
			await _unitOfWork.Chats.Save();
			return true;
		}

	}
}
