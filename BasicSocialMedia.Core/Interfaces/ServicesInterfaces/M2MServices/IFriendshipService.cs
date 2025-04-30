using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IFriendshipService
	{
		Task<SendFriendRequestDto?> GetByIdAsync(int id);
		Task<IEnumerable<GetFriendsDto>> GetAllFriendsAsync(string userId);
		Task<IEnumerable<GetFriendsDto>> GetSentFriendRequestsAsync(string userId);
		Task<IEnumerable<GetFriendsDto>> GetPendingFriendRequestsAsync(string userId);
		Task<bool> SendFriendRequestAsync(SendFriendRequestDto requestDto);
		Task<Friendship?> DoesFriendshipExist(string userId1, string userId2);
		Task<bool> RemoveFriendAsync(int friendshipId);
		Task<bool> RemoveFriendAsync(string friendId);
		Task<bool> AcceptRequestAsync(int friendshipId);
		Task<bool> RejectRequestAsync(int friendshipId);
		Task<bool> SetUserIdToNull(string userId);


	}
}
