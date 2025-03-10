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
		public async Task CreateCommentAsync(AddCommentDto commentDto)
		{
			Comment comment = new()
			{
				Content = commentDto.Content,
				UserId = commentDto.UserId,
				PostId = commentDto.PostId
			};

			await _unitOfWork.Comments.AddAsync(comment);
			await _unitOfWork.Comments.Save();
			await Task.CompletedTask;
		}
		public async Task UpdateCommentAsync(UpdateCommentDto commentDto)
		{
			Comment comment = new()
			{
				Content = commentDto.Content,
			};

			_unitOfWork.Comments.Update(comment);
			await _unitOfWork.Comments.Save();
			await Task.CompletedTask;
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
			if (comments == null || !comments.Any()) return new List<GetCommentDto>();
			List<GetCommentDto> commentsDto = [];
			foreach (var comment in comments)
			{
				commentsDto.Add(_mapper.Map<GetCommentDto>(comment));
			}
			return commentsDto;
		}
	}
}
