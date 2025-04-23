using BasicSocialMedia.Core.DTOs.MessageDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IMessagesServices
	{
		Task<MessageExistDto?> GetMessageByIdAsync(int messageId);
		Task<IEnumerable<GetMessagesDto>> GetMessagesByChatIdAsync(int chatId, string userId);
		Task<bool> CreateMessageAsync(AddMessageDto message);
		Task<bool> UpdateMessageAsync(UpdateMessageDto message);
		Task<bool> DeleteMessageAsync(int messageId);
	}
}
