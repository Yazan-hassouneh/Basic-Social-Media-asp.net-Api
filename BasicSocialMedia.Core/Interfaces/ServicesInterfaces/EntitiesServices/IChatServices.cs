using BasicSocialMedia.Core.DTOs.ChatDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface IChatServices
	{
		Task<Dictionary<string, string>?> GetUsersId(int chatId);
		Task<IEnumerable<GetChatDto>?> GetChatsByUserIdAsync(string userId);
		Task<IEnumerable<string>?> GetFilesByChatIdAsync(int chatId);
		Task<GetChatDto?> GetChatByIdAsync(int chatId, string userId);
		Task<AddChatDto> CreateChatAsync(AddChatDto chat);
		Task<bool> DeleteChatAsync(int chatId, string userId);
	}
}
