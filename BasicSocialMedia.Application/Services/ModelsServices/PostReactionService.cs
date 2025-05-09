using AutoMapper;
using BasicSocialMedia.Application.BackgroundJobs;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;
using Hangfire;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class PostReactionService(IUnitOfWork unitOfWork, IMapper mapper) : IPostReactionService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public Task<string?> GetUserId(int postReactionId)
		{
			return _unitOfWork.PostReactions.GetUserId(postReactionId);
		}
		public async Task<IEnumerable<GetReactionDto>> GetPostReactionsByPostIdAsync(int postId)
		{
			IEnumerable<PostReaction?> postReactions = await _unitOfWork.PostReactions.GetAllAsync(postId);
			IEnumerable<PostReaction?> NonNullPostReactions = postReactions.Where(postReaction => postReaction != null);

			if (NonNullPostReactions == null || !NonNullPostReactions.Any()) return Enumerable.Empty<GetReactionDto>();

			return _mapper.Map<IEnumerable<GetReactionDto>>(NonNullPostReactions);
		}
		public async Task<AddPostReactionDto> CreatePostReactionAsync(AddPostReactionDto postReactionDto)
		{
			PostReaction postReaction = new()
			{
				UserId = postReactionDto.UserId,
				PostId = postReactionDto.PostId
			};

			await _unitOfWork.PostReactions.AddAsync(postReaction);
			int effectedRows = await _unitOfWork.PostReactions.Save();
			if (effectedRows > 0)
			{
				// Add Notification
				BackgroundJob.Enqueue<NotificationBackgroundJobs>(x => x.SendPostReactionNotification(postReaction));
			}
			await Task.CompletedTask;
			return postReactionDto;
		}
		public async Task<bool> DeletePostReactionAsync(int reactionId)
		{
			PostReaction? postReaction = await _unitOfWork.PostReactions.GetByIdAsync(reactionId);
			if (postReaction == null) return false;
			_unitOfWork.PostReactions.Delete(postReaction);
			await _unitOfWork.PostReactions.Save();
			return true;
		}
		public async Task<bool> DeletePostReactionsByPostIdAsync(int postId)
		{
			try
			{
				IEnumerable<PostReaction?> postReaction = await _unitOfWork.PostReactions.FindAllWithTrackingAsync(reaction => reaction.PostId == postId);
				IEnumerable<PostReaction> NonNullPostReaction = postReaction.Where(reaction => reaction != null).Select(reaction => reaction!).ToList();

				// delete from database
				_unitOfWork.PostReactions.DeleteRange(NonNullPostReaction);
				await _unitOfWork.PostReactions.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeletePostReactionsByUserIdAsync(string userId)
		{
			try
			{
				IEnumerable<PostReaction?> postReaction = await _unitOfWork.PostReactions.FindAllWithTrackingAsync(reaction => reaction.UserId == userId);
				IEnumerable<PostReaction> NonNullPostReaction = postReaction.Where(reaction => reaction != null).Select(reaction => reaction!).ToList();

				// delete from database
				_unitOfWork.PostReactions.DeleteRange(NonNullPostReaction);
				await _unitOfWork.PostReactions.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
