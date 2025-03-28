using BasicSocialMedia.Application.Services.M2MServices;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FriendshipController(IFriendshipService friendshipService, IValidator<SendFriendRequestDto> sendFriendRequestValidator, IBlockService blockService) : ControllerBase
	{
		private readonly IFriendshipService _friendshipService = friendshipService;
		private readonly IBlockService _blockService = blockService;

		private readonly IValidator<SendFriendRequestDto> _sendFriendRequestValidator = sendFriendRequestValidator;

		[HttpGet]
		[Route("getAllFriends/{userId}")]
		public async Task<IActionResult> GetAllFriendsAsync(string userId)
		{
			var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || ClaimUserId != userId) return Unauthorized();
			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetAllFriendsAsync(userId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}

		[HttpGet]
		[Route("getSentFriendRequests/{userId}")]
		public async Task<IActionResult> GetSentFriendRequestsAsync(string userId)
		{
			var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || ClaimUserId != userId) return Unauthorized();
			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetSentFriendRequestsAsync(ClaimUserId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}		
		
		[HttpGet]
		[Route("getPendingFriendRequests/{userId}")]
		public async Task<IActionResult> GetPendingFriendRequestsAsync(string userId)
		{
			var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || ClaimUserId != userId) return Unauthorized();
			IEnumerable<GetFriendsDto> friends = await _friendshipService.GetPendingFriendRequestsAsync(userId);
			if (friends == null) return BadRequest();
			return Ok(friends);
		}

		[HttpPost]
		[Route("sendFriendRequest")]
		public async Task<IActionResult> SendFriendRequestAsync([FromBody] SendFriendRequestDto requestDto)
		{
			bool isBlocked = await _blockService.IsUsersBlocked(requestDto.SenderId, requestDto.ReceiverId);
			if (isBlocked) return BadRequest();
			var result = _sendFriendRequestValidator.Validate(requestDto);
			if (!result.IsValid) return BadRequest(result.Errors);
			
			bool friendRequest = await _friendshipService.SendFriendRequestAsync(requestDto);
			return Ok(friendRequest);
		}

		[HttpPut]
		[Route("acceptRequest/{friendshipId}")]
		public async Task<IActionResult> AcceptRequestAsync(int friendshipId)
		{
			bool result = await _friendshipService.AcceptRequestAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be accepted." });

			return Ok(new { Message = "Friend request accepted successfully." });
		}

		[HttpDelete]
		[Route("rejectRequest/{friendshipId}")]
		public async Task<IActionResult> RejectRequestAsync(int friendshipId)
		{
			bool result = await _friendshipService.RejectRequestAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be rejected." });

			return Ok(new { Message = "Friend request rejected successfully." });
		}

		[HttpDelete]
		[Route("removeFriend/{friendshipId}")]
		public async Task<IActionResult> RemoveFriendAsync(int friendshipId)
		{
			bool result = await _friendshipService.RemoveFriendAsync(friendshipId);
			if (!result) return NotFound(new { Message = "Friend request not found or cannot be deleted." });

			return Ok(new { Message = "Friend request deleted successfully." });
		}
	}
}
