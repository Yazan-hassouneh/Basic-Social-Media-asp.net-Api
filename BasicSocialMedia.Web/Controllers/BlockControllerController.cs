using BasicSocialMedia.Application.Services.M2MServices;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlockControllerController(IBlockService blockService, IFollowService followService, IFriendshipService friendshipService) : ControllerBase
	{
		private readonly IBlockService _blockService = blockService;
		private readonly IFollowService _followService = followService;
		private readonly IFriendshipService _friendshipService = friendshipService;

		[HttpPost("block")]
		public async Task<IActionResult> BlockUser([FromBody] BlockUserDto request)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || userId != request.BlockerId) return Unauthorized();

			var block = new BlockUserDto
			{
				BlockerId = userId,
				BlockedId = request.BlockedId
			};

			await _followService.CancelFollowingAsync(request.BlockedId);
			await _friendshipService.RemoveFriendAsync(request.BlockedId);
			await _blockService.BlockUserAsync(block);
			return Ok(new { message = "User blocked successfully" });
		}

		[HttpPost("unblock/{blockingId}")]
		public async Task<IActionResult> UnBlockUserAsync(int blockingId)
		{
			var result = await _blockService.UnBlockUserAsync(blockingId);
			if (result) return Ok(new { message = "User unblocked successfully" });
			return BadRequest(new { message = "Failed to unblock user" });
		}

		[HttpGet("blocklist/{userId}")]
		public async Task<IActionResult> GetBlockListAsync(string userId)
		{
			var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || ClaimUserId != userId) return Unauthorized();
			var blockList = await _blockService.GetBlockListAsync(ClaimUserId);
			if (blockList is null) return BadRequest();
			return Ok(blockList);
		}
	}
}
