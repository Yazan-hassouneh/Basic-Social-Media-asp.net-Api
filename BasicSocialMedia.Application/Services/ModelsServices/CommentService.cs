using AutoMapper;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		public async Task<IEnumerable<GetCommentDto>> GetCommentsByPostIdAsync(int postId)
		{
			IEnumerable<Comment?> comments = await _unitOfWork.Comments.GetAllAsync(postId);
			return getCommentDTOs(comments);
		}
		public async Task<IEnumerable<GetCommentDto>> GetCommentsByUserIdAsync(string userId)
		{
			IEnumerable<Comment?> comments = await _unitOfWork.Comments.FindAllAsync(comment => comment.UserId == userId);
			return getCommentDTOs(comments);
		}
		public async Task<AddCommentDto> CreateCommentAsync(AddCommentDto commentDto)
		{
			Comment comment = new()
			{
				Content = commentDto.Content,
				UserId = commentDto.UserId,
				PostId = commentDto.PostId
			};

			await _unitOfWork.Comments.AddAsync(comment);
			await _unitOfWork.Comments.Save();
			return commentDto;
		}
		public async Task<UpdateCommentDto?> UpdateCommentAsync(UpdateCommentDto commentDto)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentDto.Id);
			if (comment == null) return null;
			comment.Content = commentDto.Content;

			_unitOfWork.Comments.Update(comment);
			await _unitOfWork.Comments.Save();
			return commentDto;
		}
		public async Task<bool> DeleteComment(int commentId)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
			if (comment == null) return false;
			_unitOfWork.Comments.Delete(comment);
			await _unitOfWork.Comments.Save();
			return true;
		}
		private IEnumerable<GetCommentDto> getCommentDTOs(IEnumerable<Comment?> comments)
		{
			if (comments == null || !comments.Any()) return [];
			List<GetCommentDto> commentsDto = comments.Select(_mapper.Map<GetCommentDto>).ToList();
			return commentsDto;
		}
	}
}
