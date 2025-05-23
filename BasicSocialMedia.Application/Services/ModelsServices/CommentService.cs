﻿using AutoMapper;
using BasicSocialMedia.Application.BackgroundJobs;
using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using Ganss.Xss;
using Hangfire;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, HtmlSanitizer sanitizer, ICommentFileModeService commentFileModeService, ICommentReactionService commentReactionService) : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		private readonly HtmlSanitizer _sanitizer = sanitizer;
		private readonly ICommentFileModeService _commentFileModeService = commentFileModeService;
		private readonly ICommentReactionService _commentReactionService = commentReactionService;

		public async Task<GetCommentDto> GetCommentByIdAsync(int commentId)
		{
			Core.Models.MainModels.Comment? comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
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
			IEnumerable<Core.Models.MainModels.Comment?> comments = await _unitOfWork.Comments.GetAllAsync(postId);
			return getCommentDTOs(comments);
		}
		public async Task<IEnumerable<GetCommentDto>> GetCommentsByUserIdAsync(string userId)
		{
			IEnumerable<Core.Models.MainModels.Comment?> comments = await _unitOfWork.Comments.FindAllAsync(comment => comment.UserId == userId);
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
			
			if (effectedRows > 0)
			{
				// Add Notification
				BackgroundJob.Enqueue<NotificationBackgroundJobs>(x => x.SendNewCommentNotification(comment));
				if (commentDto.Files.Count > 0)
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

			}
			return commentDto;
		}
		public async Task<UpdateCommentDto?> UpdateCommentAsync(UpdateCommentDto commentDto)
		{
			CancellationToken cancellationToken = new();
			bool isCommentExist = await _unitOfWork.Comments.DoesExist(commentDto.Id, cancellationToken);
			if (!isCommentExist) return null;

			Core.Models.MainModels.Comment? comment = await _unitOfWork.Comments.GetByIdWithTrackingAsync(commentDto.Id);
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
			if (comment is null) return false;

			List<string> files = await GetCommentFiles(comment.Id);

			try
			{
				// First delete related entities
				bool isRelatedEntitiesDeleted = await IsRelatedEntitiesDeleted(files, commentId);
				if (!isRelatedEntitiesDeleted) return false;
				// Then delete the Comment
				_unitOfWork.Comments.Delete(comment);
				int effectedRows = await _unitOfWork.Comments.Save();

				if (effectedRows == 0) throw new Exception("Comment  deletion failed.");

				return true;
			}
			catch (Exception ex)
			{
				// Log error here if needed
				return false;
			}
		}		
		public async Task<bool> DeleteCommentsByPostIdAsync(int postId)
		{
			IEnumerable<Comment?> comments = await _unitOfWork.Comments.FindAllWithTrackingAsync(comment => comment.PostId == postId);
			IEnumerable<Comment> nonNullableComments = comments.Where(comment => comment != null).Select(comment => comment!);
			if (comments == null) return false;

			try
			{

				foreach (var comment in nonNullableComments)
				{
					List<string> files = await GetCommentFiles(comment.Id);
					bool isRelatedEntitiesDeleted = await IsRelatedEntitiesDeleted(files, comment.Id);
					if (!isRelatedEntitiesDeleted) return false;

					_unitOfWork.Comments.Delete(comment);
					int effectedRows = await _unitOfWork.Comments.Save();
					if (effectedRows == 0) throw new Exception("Comment  deletion failed.");
				}

				return true;
			}
			catch (Exception ex)
			{
				// Log error here if needed
				return false;
			}
		}
		public async Task<bool> DeleteCommentsByUserIdAsync(string userId)
		{
			IEnumerable<Comment?> comments = await _unitOfWork.Comments.FindAllWithTrackingAsync(comment => comment.UserId == userId);
			IEnumerable<Comment> nonNullableComments = comments.Where(comment => comment != null).Select(comment => comment!);
			if (comments == null) return false;

			try
			{
				foreach (var comment in nonNullableComments)
				{
					List<string> files = await GetCommentFiles(comment.Id);
					bool isRelatedEntitiesDeleted = await IsRelatedEntitiesDeleted(files, comment.Id);
					if (!isRelatedEntitiesDeleted) return false;

					_unitOfWork.Comments.Delete(comment);
					int effectedRows = await _unitOfWork.Comments.Save();
					if (effectedRows == 0) throw new Exception("Comment  deletion failed.");
				}

				return true;
			}
			catch (Exception ex)
			{
				// Log error here if needed
				return false;
			}
		}

		//Helper Functions
		//
		private IEnumerable<GetCommentDto> getCommentDTOs(IEnumerable<Comment?> comments)
		{
			if (comments == null || !comments.Any()) return [];
			List<GetCommentDto> commentsDto = comments.Select(_mapper.Map<GetCommentDto>).ToList();
			return commentsDto;
		}
		private async Task<bool> IsRelatedEntitiesDeleted(List<string> files, int commentId)
		{
			// The Files Entities Remove Automatically By The Database
			bool isFileDeleted = _commentFileModeService.DeleteCommentFiles(files);
			bool isReactionDeleted = await _commentReactionService.DeleteCommentReactionsByCommentIdAsync(commentId);

			if (!isReactionDeleted || !isFileDeleted)
			{
				return false;
			}
			return true;
		}
		private async Task<List<string>> GetCommentFiles(int commentId)
		{
			return (await _unitOfWork.CommentFiles.GetAllAsync(file => file.CommentId == commentId)).Select(file => file!.Path).ToList();

		}
	}
}
