using AutoMapper;
using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;
using Ganss.Xss;
using Microsoft.Security.Application;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, HtmlSanitizer sanitizer) : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly HtmlSanitizer _sanitizer = sanitizer;

		public async Task<GetCommentDto> GetCommentByIdAsync(int commentId)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
			if (comment == null) return new GetCommentDto();
			GetCommentDto commentDto = _mapper.Map<GetCommentDto>(comment);
			return commentDto;
		}
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
			CancellationToken cancellationToken = new();
			bool isCommentExist = await _unitOfWork.Comments.DoesExist(commentDto.Id, cancellationToken);
			if (!isCommentExist) return null;

			Comment? comment = await _unitOfWork.Comments.GetByIdWithTrackingAsync(commentDto.Id);
			if (comment == null) return null;

			byte[] providedRowVersionBytes = Convert.FromBase64String(commentDto.RowVersion);
			if (!Compare.ByteArrayCompare(comment.RowVersion, providedRowVersionBytes))
			{
				// Row versions don't match, indicating a concurrency conflict
				// You should handle this appropriately, e.g., throw an exception or return a specific error code
				throw new Exception("Concurrency conflict: The comment has been modified by another user.");
			}

			comment.Content = _sanitizer.Sanitize(commentDto.Content);

			_unitOfWork.Comments.Update(comment);
			await _unitOfWork.Comments.Save();
			return commentDto;
		}
		public async Task<bool> DeleteCommentAsync(int commentId)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
			if (comment == null) return false;
			_unitOfWork.Comments.Delete(comment);
			await _unitOfWork.Comments.Save();
			return true;
		}

		//Helper Functions
		//
		private IEnumerable<GetCommentDto> getCommentDTOs(IEnumerable<Comment?> comments)
		{
			if (comments == null || !comments.Any()) return [];
			List<GetCommentDto> commentsDto = comments.Select(_mapper.Map<GetCommentDto>).ToList();
			return commentsDto;
		}
	}
}
