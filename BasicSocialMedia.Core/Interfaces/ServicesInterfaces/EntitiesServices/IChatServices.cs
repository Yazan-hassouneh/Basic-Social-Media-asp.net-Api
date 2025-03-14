using BasicSocialMedia.Core.DTOs.ChatDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IChatServices
	{
		Task<IEnumerable<GetChatDto>?> GetChatsByUserIdAsync(string userId);
		Task<GetChatDto?> GetChatByIdAsync(int chatId);
		Task<AddChatDto> CreateChatAsync(AddChatDto chat);
		Task<bool> DeleteChatAsync(int chatId);
	}
}
