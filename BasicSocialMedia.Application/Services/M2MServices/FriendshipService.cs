using AutoMapper;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Services.M2MServices
{
	public class FriendshipService(IUnitOfWork unitOfWork, IMapper mapper) : IFriendshipService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public async Task<IEnumerable<GetFriendsDto>> GetSentFriendRequestsAsync(string userId)
		{
			IEnumerable<Friendship?> friends = await _unitOfWork.Friendship.GetAllSentFriendRequestsAsync(userId);
			if (IsFriendListNullOrEmpty(friends)) return Enumerable.Empty<GetFriendsDto>();

			return MapFriendList(friends, ProjectEnums.FriendshipStatus.Pending);
		}		
		public async Task<IEnumerable<GetFriendsDto>> GetPendingFriendRequestsAsync(string userId)
		{
			IEnumerable<Friendship?> friends = await _unitOfWork.Friendship.GetAllPendingFriendRequestsAsync(userId);
			if (IsFriendListNullOrEmpty(friends)) return Enumerable.Empty<GetFriendsDto>();

			return MapFriendList(friends, ProjectEnums.FriendshipStatus.Pending);
		}
		public async Task<IEnumerable<GetFriendsDto>> GetAllFriendsAsync(string userId)
		{
			IEnumerable<Friendship?> friends = await _unitOfWork.Friendship.GetAllFriendsAsync(userId);
			if (friends is null || !friends.Any()) return Enumerable.Empty<GetFriendsDto>();

			return MapFriendList(friends, ProjectEnums.FriendshipStatus.Accepted);

		}
		public async Task<bool> SendFriendRequestAsync(SendFriendRequestDto requestDto)
		{
			var existingRequest = await DoesFriendshipExist(requestDto);
			if (existingRequest != null) return false;

			// Create a new friend request
			Friendship friendRequest = new()
			{
				SenderId = requestDto.SenderId,
				ReceiverId = requestDto.ReceiverId,
			};

			await _unitOfWork.Friendship.AddAsync(friendRequest);
			await _unitOfWork.Friendship.Save();
			return true;
		}
		public async Task<bool> AcceptRequestAsync(int friendshipId)
		{
			CancellationToken cancellationToken = new();
			bool isFriendshipExist = await _unitOfWork.Friendship.DoesExist(friendshipId, cancellationToken);
			if (!isFriendshipExist) return false; 
			
			Friendship? friendship = await _unitOfWork.Friendship.GetByIdWithTrackingAsync(friendshipId);
			if (friendship == null) return false;
			
			friendship.Status = ProjectEnums.FriendshipStatus.Accepted;

			_unitOfWork.Friendship.Update(friendship);
			await _unitOfWork.Friendship.Save();
			return true;
		}
		public async Task<bool> RejectRequestAsync(int friendshipId)
		{
			CancellationToken cancellationToken = new();
			bool isFriendshipExist = await _unitOfWork.Friendship.DoesExist(friendshipId, cancellationToken);
			if (!isFriendshipExist) return false;

			Friendship? friendship = await _unitOfWork.Friendship.GetByIdAsync(friendshipId);
			if (friendship == null) return false;

			_unitOfWork.Friendship.Delete(friendship);
			await _unitOfWork.Friendship.Save();
			return true;
		}
		public async Task<bool> RemoveFriendAsync(int friendshipId)
		{
			Friendship? friendship = await _unitOfWork.Friendship.GetByIdAsync(friendshipId);
			if (friendship == null) return false;
			_unitOfWork.Friendship.Delete(friendship);
			await _unitOfWork.Friendship.Save();
			return true;
		}
		public async Task<bool> RemoveFriendAsync(string friendId)
		{
			Friendship? friendship = await _unitOfWork.Friendship.FindWithTrackingAsync(friendship => friendship.SenderId == friendId || friendship.ReceiverId == friendId);
			if (friendship == null) return false;
			_unitOfWork.Friendship.Delete(friendship);
			await _unitOfWork.Friendship.Save();
			return true;
		}
		private async Task<Friendship?> DoesFriendshipExist(SendFriendRequestDto requestDto)
		{
			return await _unitOfWork.Friendship.FindAsync(request =>
					request.SenderId == requestDto.SenderId && request.ReceiverId == requestDto.ReceiverId ||
					request.ReceiverId == requestDto.SenderId && request.SenderId == requestDto.ReceiverId);
		}
		private static bool IsFriendListNullOrEmpty(IEnumerable<Friendship?> friends)
		{
			return friends is null || !friends.Any();
		}
		private List<GetFriendsDto> MapFriendList(IEnumerable<Friendship?> friends, ProjectEnums.FriendshipStatus friendshipStatus)
		{
			return friends
				.Where(friend => friend is not null && friend.Status == friendshipStatus)
				.Select(friend => _mapper.Map<GetFriendsDto>(friend!))
				.ToList();
		}
	}
}
