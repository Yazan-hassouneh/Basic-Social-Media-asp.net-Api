using BasicSocialMedia.Core.DTOs.FileModelsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices
{
	public interface IMessageFileModelService
	{
		Task<IEnumerable<string>> GetAllFilesByMessageIdAsync(int messageId);
		Task<IEnumerable<string>> GetAllFilesByChatIdAsync(int chatId);
		Task<IEnumerable<string>> GetAllFilesByUserIdAsync(string userId);
		Task<bool> AddMessageFileAsync(AddMessageFileDto addMessageFilesDto);
		Task<bool> UpdateMessageFileAsync(UpdateMessageFileDto updateMessageFileDto);
		Task<bool> DeleteMessageFileByFileIdAsync(int fileId);
		Task<bool> DeleteMessageFileByMessageIdAsync(int messageId);
		Task<bool> DeleteMessageFileByChatIdAsync(int chatId);
	}
}
