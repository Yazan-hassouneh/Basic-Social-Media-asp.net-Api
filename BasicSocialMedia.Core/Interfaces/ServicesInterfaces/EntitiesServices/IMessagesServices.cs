using BasicSocialMedia.Core.DTOs.MessageDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IMessagesServices
	{
		Task<string?> GetUserId(int messageId);
		Task<IEnumerable<GetMessagesDto>> GetMessagesByChatIdAsync(int chatId, string userId);
		Task<AddMessageDto> CreateMessageAsync(AddMessageDto message);
		Task<UpdateMessageDto?> UpdateMessageAsync(UpdateMessageDto message);
		Task<bool> DeleteMessageAsync(int messageId);
	}
}
