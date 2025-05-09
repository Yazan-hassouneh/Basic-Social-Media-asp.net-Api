using AutoMapper;
using BasicSocialMedia.Application.BackgroundJobs;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;
using Hangfire;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentReactionService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentReactionService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public Task<string?> GetUserId(int commentReactionId)
		{
			return _unitOfWork.CommentReactions.GetUserId(commentReactionId);
		}
		public async Task<IEnumerable<GetReactionDto>> GetCommentReactionsByCommentIdAsync(int commentId)
		{
			IEnumerable<CommentReaction?> commentReactions = await _unitOfWork.CommentReactions.GetAllAsync(commentId);
			IEnumerable<CommentReaction?> NonNullCommentReactions = commentReactions.Where(commentReaction => commentReaction != null);

			if (NonNullCommentReactions == null || !NonNullCommentReactions.Any()) return Enumerable.Empty<GetReactionDto>();

			return _mapper.Map<IEnumerable<GetReactionDto>>(NonNullCommentReactions);
		}
		public async Task<AddCommentReactionDto> CreateCommentReactionAsync(AddCommentReactionDto commentReactionDto)
		{
			CommentReaction commentReaction = new()
			{
				UserId = commentReactionDto.UserId,
				CommentId = commentReactionDto.CommentId,
			};

			await _unitOfWork.CommentReactions.AddAsync(commentReaction);
			int effectedRows = await _unitOfWork.CommentReactions.Save();
			if (effectedRows > 0)
			{
				// Add Notification
				BackgroundJob.Enqueue<NotificationBackgroundJobs>(x => x.SendCommentReactionNotification(commentReaction));
			}
			await Task.CompletedTask;
			return commentReactionDto;
		}
		public async Task<bool> DeleteCommentReaction(int reactionId)
		{
			CommentReaction? commentReaction = await _unitOfWork.CommentReactions.GetByIdAsync(reactionId);
			if (commentReaction == null) return false;
			_unitOfWork.CommentReactions.Delete(commentReaction);
			await _unitOfWork.CommentReactions.Save();
			return true;
		}
		public async Task<bool> DeleteCommentReactionsByCommentIdAsync(int commentId)
		{
			try
			{
				IEnumerable<CommentReaction?> commentReaction = await _unitOfWork.CommentReactions.FindAllWithTrackingAsync(reaction => reaction.CommentId == commentId);
				IEnumerable<CommentReaction> NonNullCommentReaction = commentReaction.Where(reaction => reaction != null).Select(reaction => reaction!).ToList();

				// delete from database
				_unitOfWork.CommentReactions.DeleteRange(NonNullCommentReaction);
				await _unitOfWork.CommentReactions.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteCommentReactionsByUserIdAsync(string userId)
		{
			try
			{
				IEnumerable<CommentReaction?> commentReaction = await _unitOfWork.CommentReactions.FindAllWithTrackingAsync(reaction => reaction.UserId == userId);
				IEnumerable<CommentReaction> NonNullCommentReaction = commentReaction.Where(reaction => reaction != null).Select(reaction => reaction!).ToList();

				// delete from database
				_unitOfWork.CommentReactions.DeleteRange(NonNullCommentReaction);
				await _unitOfWork.CommentReactions.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
