using BasicSocialMedia.Core.DTOs.Comment;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface ICommentService
	{
		Task<GetCommentDto> GetCommentByIdAsync(int commentId);
		Task<string?> GetUserId(int commentId);
		Task<IEnumerable<GetCommentDto>> GetCommentsByPostIdAsync(int postId);
		Task<IEnumerable<GetCommentDto>> GetCommentsByUserIdAsync(string userId);
		Task<AddCommentDto> CreateCommentAsync(AddCommentDto commentDto);
		Task<UpdateCommentDto?> UpdateCommentAsync(UpdateCommentDto commentDto);
		Task<bool> DeleteCommentAsync(int commentId);
		Task<bool> DeleteCommentsByPostIdAsync(int postId);
		Task<bool> DeleteCommentsByUserIdAsync(string userId);
	}
}
