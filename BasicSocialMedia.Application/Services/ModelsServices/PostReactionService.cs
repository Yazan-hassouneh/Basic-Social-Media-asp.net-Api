using AutoMapper;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Services.ModelsServices
{
	public class PostReactionService(IUnitOfWork unitOfWork, IMapper mapper) : IPostReactionService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;
		public async Task<IEnumerable<GetReactionDto>> GetPostReactionsByPostIdAsync(int postId)
		{
			IEnumerable<PostReaction?> postReactions = await _unitOfWork.PostReactions.FindAllAsync(postReaction => postReaction.PostId == postId);
			IEnumerable<PostReaction?> NonNullPostReactions = postReactions.Where(postReaction => postReaction != null);

			if (NonNullPostReactions == null || !NonNullPostReactions.Any()) return Enumerable.Empty<GetReactionDto>();

			return _mapper.Map<IEnumerable<GetReactionDto>>(NonNullPostReactions);
		}
		public async Task CreatePostReactionAsync(AddPostReactionDto postReactionDto)
		{
			PostReaction postReaction = new()
			{
				UserId = postReactionDto.UserId,
				PostId = postReactionDto.PostId
			};

			await _unitOfWork.PostReactions.AddAsync(postReaction);
			await _unitOfWork.PostReactions.Save();
			await Task.CompletedTask;
		}
		public async Task<bool> DeletePostAsync(int reactionId)
		{
			PostReaction? postReaction = await _unitOfWork.PostReactions.GetByIdAsync(reactionId);
			if (postReaction == null) return false;
			_unitOfWork.PostReactions.Delete(postReaction);
			await _unitOfWork.PostReactions.Save();
			return true;
		}
	}
}
