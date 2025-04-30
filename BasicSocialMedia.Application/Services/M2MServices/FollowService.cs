using AutoMapper;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Services.M2MServices
{
	public class FollowService(IUnitOfWork unitOfWork, IMapper mapper) : IFollowService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public async Task<SendFollowRequestDto?> GetByIdAsync(int FollowId)
		{
			Follow? follow = await _unitOfWork.Following.GetByIdAsync(FollowId);
			if (follow is null) return null;

			SendFollowRequestDto result = new()
			{
				FollowerId = follow.FollowerId,
				FollowingId = follow.FollowerId,
			};

			return result;
		}		
		public async Task<IEnumerable<GetFollowerDto>> GetAllFollowersAsync(string userId)
		{
			IEnumerable<Follow?> followers = await _unitOfWork.Following.GetAllFollowersAsync(userId);
			if (followers is null || !followers.Any()) return Enumerable.Empty<GetFollowerDto>();

			List<GetFollowerDto> followersDto = followers
				.Where(follower => follower is not null)
				.Select(follower => _mapper.Map<GetFollowerDto>(follower!))
				.ToList();

			return followersDto;
		}
		public async Task<IEnumerable<GetFollowingDto>> GetAllFollowingsAsync(string userId)
		{
			IEnumerable<Follow?> followings = await _unitOfWork.Following.GetAllFollowingsAsync(userId);

			if (followings is null || !followings.Any()) return Enumerable.Empty<GetFollowingDto>();

			List<GetFollowingDto> followingsDto = followings
				.Where(followingUser => followingUser is not null)
				.Select(followingUser => _mapper.Map<GetFollowingDto>(followingUser!))
				.ToList();

			return followingsDto;
		}
		public async Task<bool> FollowAsync(SendFollowRequestDto requestDto)
		{
			Follow follow = new()
			{
				FollowerId = requestDto.FollowerId,
				FollowingId = requestDto.FollowingId,
			};

			await _unitOfWork.Following.AddAsync(follow);
			await _unitOfWork.Following.Save();
			return true;
		}
		public async Task<bool> CancelFollowingAsync(int followId)
		{ 
			Follow? follow = await _unitOfWork.Following.GetByIdAsync(followId);
			if (follow == null) return true;
			_unitOfWork.Following.Delete(follow);
			await _unitOfWork.Following.Save();
			return true;
		}		
		public async Task<bool> CancelAllFollowingsByFollowingIdAsync(string followingId)
		{ 
			IEnumerable<Follow?> follows = await _unitOfWork.Following.FindAllWithTrackingAsync(follow => follow.FollowingId == followingId);
			IEnumerable<Follow> nonNullableFollows = follows.Where(follow => follow != null).Select(follow => follow!);
			_unitOfWork.Following.DeleteRange(nonNullableFollows);
			await _unitOfWork.Following.Save();
			return true;
		}
		public async Task<bool> SetUserIdToNull(string userId)
		{
			var follows = await _unitOfWork.Following.FindAllWithTrackingAsync(follow => follow.FollowingId == userId || follow.FollowerId == userId);
			if (follows == null || !follows.Any()) return true;

			if (follows.Any())
			{
				foreach (var follow in follows)
				{
					bool updated = false;

					if (follow!.FollowerId == userId)
					{
						follow.FollowerId = null;
						updated = true;
					}

					if (follow.FollowingId == userId)
					{
						follow.FollowingId = null;
						updated = true;
					}

					if (updated) _unitOfWork.Following.Update(follow);
				}

				await _unitOfWork.Following.Save();
				return true;
			}
			return true;
		}
	}
}
