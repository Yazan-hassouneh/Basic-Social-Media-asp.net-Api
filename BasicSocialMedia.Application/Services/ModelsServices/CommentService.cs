using AutoMapper;
using BasicSocialMedia.Application.Services.FileModelServices;
using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using Ganss.Xss;
using Microsoft.Extensions.Hosting;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, HtmlSanitizer sanitizer, ICommentFileModeService commentFileModeService) : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly HtmlSanitizer _sanitizer = sanitizer;
		private readonly ICommentFileModeService _commentFileModeService = commentFileModeService;

		public async Task<GetCommentDto> GetCommentByIdAsync(int commentId)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
			if (comment == null) return new GetCommentDto();
			GetCommentDto commentDto = _mapper.Map<GetCommentDto>(comment);
			return commentDto;
		}
		public Task<string?> GetUserId(int commentId)
		{
			return _unitOfWork.Comments.GetUserId(commentId);
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
			int effectedRows = await _unitOfWork.Comments.Save();
			
			if (commentDto.Files.Count > 0 && effectedRows > 0)
			{
				AddCommentFileDto addCommentFileDto = new()
				{
					UserId = commentDto.UserId,
					CommentId = comment.Id,
					PostId = commentDto.PostId,
					Files = commentDto.Files,
				};

				bool mediaSaved = await _commentFileModeService.AddCommentFileAsync(addCommentFileDto);
				if (!mediaSaved)
				{
					_unitOfWork.Comments.Delete(comment);
					await _unitOfWork.Comments.Save();
					return new AddCommentDto();
				}
			}
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

			bool hasNewFiles = commentDto.Files is not null && commentDto.Files.Count > 0;
			List<string>? oldMediaPath = commentDto.MediaPaths ?? [];
			IEnumerable<CommentFileModel> commentFileModels = (await _unitOfWork.CommentFiles.FindAllAsync(file => file.CommentId == commentDto.Id)).Where(file => file != null)!;

			UpdateCommentFileDto? updateCommentFileDto = null;
			if (hasNewFiles || oldMediaPath!.Count > 0)
			{
				updateCommentFileDto = new UpdateCommentFileDto()
				{
					UserId = commentDto!.UserId,
					CommentId = comment.Id,
					PostId = comment.PostId,
					Files = commentDto.Files,
					MediaPaths = commentDto.MediaPaths,
				};
			}

			comment.Content = _sanitizer.Sanitize(commentDto.Content);

			_unitOfWork.Comments.Update(comment);
			int effectedRows = await _unitOfWork.Comments.Save();

			if (effectedRows > 0)
			{
				if (updateCommentFileDto != null)
				{
					await _commentFileModeService.UpdateCommentFileAsync(updateCommentFileDto);
				}
				if (updateCommentFileDto is null && commentFileModels.Any())
				{
					await _commentFileModeService.DeleteCommentFileByCommentIdAsync(commentDto.Id);
				}
			}
			return commentDto;
		}
		public async Task<bool> DeleteCommentAsync(int commentId)
		{
			Comment? comment = await _unitOfWork.Comments.GetByIdWithTrackingAsync(commentId);
			if (comment == null) return false;

			List<string> files = (await _unitOfWork.CommentFiles.GetAllAsync(file => file.CommentId == comment.Id)).Select(file => file!.Path).ToList();
			_unitOfWork.Comments.Delete(comment);
			int effectedRows = await _unitOfWork.Comments.Save();
			if (effectedRows > 0) _commentFileModeService.DeleteCommentFiles(files);
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
