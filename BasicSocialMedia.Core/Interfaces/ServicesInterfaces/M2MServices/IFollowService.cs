using BasicSocialMedia.Core.DTOs.M2MDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IFollowService
	{
		Task<IEnumerable<GetFollowerDto>> GetAllFollowersAsync(string userId);
		Task<IEnumerable<GetFollowingDto>> GetAllFollowingsAsync(string userId);
		Task<bool> FollowAsync(SendFollowRequestDto requestDto);
		Task<bool> CancelFollowingAsync(int followId);
	}
}
