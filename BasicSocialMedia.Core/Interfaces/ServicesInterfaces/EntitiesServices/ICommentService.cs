using BasicSocialMedia.Core.DTOs.Comment;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices
{
	public interface ICommentService
	{
		Task<IEnumerable<GetCommentDto>> GetCommentsByPostIdAsync(int postId);
		Task<IEnumerable<GetCommentDto>> GetCommentsByUserIdAsync(string userId);
		Task<AddCommentDto> CreateCommentAsync(AddCommentDto commentDto);
		Task<UpdateCommentDto?> UpdateCommentAsync(UpdateCommentDto commentDto);
		Task<bool> DeleteComment(int commentId);
	}
}
