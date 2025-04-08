using BasicSocialMedia.Core.DTOs.FileModelsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices
{
	public interface IPostFileModelService
	{
		Task<IEnumerable<string>> GetAllFilesByPostIdAsync(int postId);
		Task<IEnumerable<string>> GetAllFilesByUserIdAsync(string userId);
		Task<bool> AddPostFileAsync(AddPostFileDto addPostFilesDto);
		Task<bool> UpdatePostFileAsync(UpdatePostFileDto updatePostFileDto);
		Task<bool> DeletePostFileByPostIdAsync(int postId);
		Task<bool> DeletePostFileByFileIdAsync(int fileId);
	}
}
