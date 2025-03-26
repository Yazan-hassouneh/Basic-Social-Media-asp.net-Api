using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Services.M2MServices
{
	public class FriendshipService(IUnitOfWork unitOfWork) : IFriendshipService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		public async Task<bool> SendFriendRequestAsync(SendFriendRequestDto requestDto)
		{
			var existingRequest = await DoesFriendshipExist(requestDto);
			if (existingRequest != null) return false;

			// Create a new friend request
			Friendship friendRequest = new()
			{
				UserId1 = requestDto.UserId1,
				UserId2 = requestDto.UserId2,
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
		public async Task<bool> BlockUserAsync(SendFriendRequestDto requestDto)
		{
			var friendship = await DoesFriendshipExist(requestDto);
			if (friendship == null)
			{
				Friendship friendRequest = new()
				{
					UserId1 = requestDto.UserId1,
					UserId2 = requestDto.UserId2,
					Status = ProjectEnums.FriendshipStatus.Blocked,
				};

				await _unitOfWork.Friendship.AddAsync(friendRequest);

			}else
			{
				friendship.Status = ProjectEnums.FriendshipStatus.Blocked;
				_unitOfWork.Friendship.Update(friendship);
			}

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
		private async Task<Friendship?> DoesFriendshipExist(SendFriendRequestDto requestDto)
		{
			return await _unitOfWork.Friendship.FindAsync(request =>
					request.UserId1 == requestDto.UserId1 && request.UserId2 == requestDto.UserId2 ||
					request.UserId2 == requestDto.UserId1 && request.UserId1 == requestDto.UserId2);
		}
	}
}
