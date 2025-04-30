using BasicSocialMedia.Core.DTOs.M2MDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IFollowService
	{
		Task<SendFollowRequestDto?> GetByIdAsync(int Follow);
		Task<IEnumerable<GetFollowerDto>> GetAllFollowersAsync(string userId);
		Task<IEnumerable<GetFollowingDto>> GetAllFollowingsAsync(string userId);
		Task<bool> FollowAsync(SendFollowRequestDto requestDto);
		Task<bool> CancelFollowingAsync(int followId);
		Task<bool> CancelFollowingAsync(string followingId);
		Task<bool> SetUserIdToNull(string userId);
	}
}
