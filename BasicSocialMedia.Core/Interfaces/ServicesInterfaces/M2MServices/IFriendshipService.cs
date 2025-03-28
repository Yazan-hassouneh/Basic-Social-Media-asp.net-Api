using BasicSocialMedia.Core.DTOs.M2MDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IFriendshipService
	{
		Task<IEnumerable<GetFriendsDto>> GetAllFriendsAsync(string userId);
		Task<IEnumerable<GetFriendsDto>> GetSentFriendRequestsAsync(string userId);
		Task<IEnumerable<GetFriendsDto>> GetPendingFriendRequestsAsync(string userId);
		Task<bool> SendFriendRequestAsync(SendFriendRequestDto requestDto);
		Task<bool> RemoveFriendAsync(int friendshipId);
		Task<bool> RemoveFriendAsync(string friendId);
		Task<bool> AcceptRequestAsync(int friendshipId);
		Task<bool> RejectRequestAsync(int friendshipId);
		
	}
}
