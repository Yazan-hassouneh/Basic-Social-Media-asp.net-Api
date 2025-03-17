using AutoMapper;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class CommentReactionService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentReactionService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<GetReactionDto>> GetCommentReactionsByCommentIdAsync(int commentId)
		{
			IEnumerable<CommentReaction?> commentReactions = await _unitOfWork.CommentReactions.FindAllAsync(commentReaction => commentReaction.CommentId == commentId);
			IEnumerable<CommentReaction?> NonNullCommentReactions = commentReactions.Where(commentReaction => commentReaction != null);

			if (NonNullCommentReactions == null || !NonNullCommentReactions.Any()) return Enumerable.Empty<GetReactionDto>();

			return _mapper.Map<IEnumerable<GetReactionDto>>(NonNullCommentReactions);
		}
		public async Task CreateCommentReactionAsync(AddCommentReactionDto commentReactionDto)
		{
			CommentReaction commentReaction = new()
			{
				UserId = commentReactionDto.UserId,
				CommentId = commentReactionDto.CommentId,
			};

			await _unitOfWork.CommentReactions.AddAsync(commentReaction);
			await _unitOfWork.CommentReactions.Save();
			await Task.CompletedTask;
		}
		public async Task<bool> DeleteCommentReaction(int reactionId)
		{
			CommentReaction? commentReaction = await _unitOfWork.CommentReactions.GetByIdAsync(reactionId);
			if (commentReaction == null) return false;
			_unitOfWork.CommentReactions.Delete(commentReaction);
			await _unitOfWork.CommentReactions.Save();
			return true;
		}
	}
}
