

using BasicSocialMedia.Core.DTOs.M2MDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IFriendshipService
	{
		Task<bool> SendFriendRequestAsync(SendFriendRequestDto requestDto);
		Task<bool> RemoveFriendAsync(int friendshipId);
		Task<bool> AcceptRequestAsync(int friendshipId);
		Task<bool> RejectRequestAsync(int friendshipId);
		Task<bool> BlockUserAsync(SendFriendRequestDto requestDto);
		
	}
}
