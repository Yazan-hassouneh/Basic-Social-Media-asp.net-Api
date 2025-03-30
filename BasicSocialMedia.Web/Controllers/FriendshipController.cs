using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FriendshipController(IFriendshipService friendshipService, IValidator<SendFriendRequestDto> sendFriendRequestValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IFriendshipService _friendshipService = friendshipService;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly IValidator<SendFriendRequestDto> _sendFriendRequestValidator = sendFriendRequestValidator;

		[HttpGet]
		[Route("getAllFriends/{userId}")]
		public async Task<IActionResult> GetAllFriendsAsync(string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetAllFriendsAsync(userId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}

		[HttpGet]
		[Route("getSentFriendRequests/{userId}")]
		public async Task<IActionResult> GetSentFriendRequestsAsync(string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetSentFriendRequestsAsync(userId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}		
		
		[HttpGet]
		[Route("getPendingFriendRequests/{userId}")]
		public async Task<IActionResult> GetPendingFriendRequestsAsync(string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetPendingFriendRequestsAsync(userId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}

		[HttpPost]
		[Route("sendFriendRequest")]
		public async Task<IActionResult> SendFriendRequestAsync([FromBody] SendFriendRequestDto requestDto)
		{
			var isCurrentUserTheOwner = await _authorizationService.AuthorizeAsync(User, requestDto.SenderId, PoliciesSettings.Ownership);
			var isUserBlocked = await _authorizationService.AuthorizeAsync(User, requestDto.ReceiverId, PoliciesSettings.IsUserBlocked);
			if (!isCurrentUserTheOwner.Succeeded && !isUserBlocked.Succeeded) return Forbid();

			var result = _sendFriendRequestValidator.Validate(requestDto);
			if (!result.IsValid) return BadRequest(result.Errors);
			
			bool friendRequest = await _friendshipService.SendFriendRequestAsync(requestDto);
			return Ok(friendRequest);
		}

		[HttpPut]
		[Route("acceptRequest/{friendshipId}")]
		public async Task<IActionResult> AcceptRequestAsync(int friendshipId)
		{
			SendFriendRequestDto? friendRequestDto = await _friendshipService.GetByIdAsync(friendshipId);
			if (friendRequestDto == null) return NotFound();

			var isCurrentUserTheOwner = await _authorizationService.AuthorizeAsync(User, friendRequestDto.ReceiverId, PoliciesSettings.Ownership); 
			var isUserBlocked = await _authorizationService.AuthorizeAsync(User, friendRequestDto.SenderId, PoliciesSettings.IsUserBlocked);
			if (!isCurrentUserTheOwner.Succeeded && !isUserBlocked.Succeeded) return Forbid();

			bool result = await _friendshipService.AcceptRequestAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be accepted." });

			return Ok(new { Message = "Friend request accepted successfully." });
		}

		[HttpDelete]
		[Route("rejectRequest/{friendshipId}")]
		public async Task<IActionResult> RejectRequestAsync(int friendshipId)
		{
			SendFriendRequestDto? friendRequestDto = await _friendshipService.GetByIdAsync(friendshipId);
			if (friendRequestDto == null) return NotFound();

			var isCurrentUserTheOwner = await _authorizationService.AuthorizeAsync(User, friendRequestDto.ReceiverId, PoliciesSettings.Ownership);
			if (!isCurrentUserTheOwner.Succeeded) return Forbid();

			bool result = await _friendshipService.RejectRequestAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be rejected." });

			return Ok(new { Message = "Friend request rejected successfully." });
		}

		[HttpDelete]
		[Route("removeFriend/{friendshipId}")]
		public async Task<IActionResult> RemoveFriendAsync(int friendshipId)
		{
			SendFriendRequestDto? friendRequestDto = await _friendshipService.GetByIdAsync(friendshipId);
			if (friendRequestDto == null) return NotFound();

			var isSenderAuthorized = await _authorizationService.AuthorizeAsync(User, friendRequestDto.ReceiverId, PoliciesSettings.Ownership);
			var isReceiverAuthorized = await _authorizationService.AuthorizeAsync(User, friendRequestDto.SenderId, PoliciesSettings.Ownership);
			if (!isReceiverAuthorized.Succeeded && !isSenderAuthorized.Succeeded) return Forbid();

			bool result = await _friendshipService.RemoveFriendAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be deleted." });

			return Ok(new { Message = "Friend request deleted successfully." });
		}
	}
}
