using BasicSocialMedia.Core.DTOs.FileModelsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices
{
	public interface ICommentFileModeService
	{
		Task<bool> AddCommentFileAsync(AddCommentFileDto addCommentFilesDto);
		Task<bool> UpdateCommentFileAsync(UpdateCommentFileDto updateCommentFileDto);
		Task<bool> DeleteCommentFileByCommentIdAsync(int commentId);
		Task<bool> DeleteCommentFileByPostIdAsync(int postId);
		Task<bool> DeleteCommentFileByFileIdAsync(int fileId);
	}
}
