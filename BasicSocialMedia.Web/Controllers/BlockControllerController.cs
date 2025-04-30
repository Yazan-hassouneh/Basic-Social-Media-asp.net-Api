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
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class BlockControllerController(IBlockService blockService, IFollowService followService, IFriendshipService friendshipService, IValidator<BlockUserDto> blockUserValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IBlockService _blockService = blockService;
		private readonly IFollowService _followService = followService;
		private readonly IFriendshipService _friendshipService = friendshipService;
		private readonly IValidator<BlockUserDto> _blockUserValidator = blockUserValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;


		[HttpPost("block")]
		public async Task<IActionResult> BlockUser([FromBody] BlockUserDto request)
		{
			var result = await _blockUserValidator.ValidateAsync(request);
			if (!result.IsValid) return BadRequest(result.Errors);

			var userId = User.FindFirst("userId")?.Value;
			if (string.IsNullOrEmpty(userId) || userId != request.BlockerId) return Unauthorized();

			var block = new BlockUserDto
			{
				BlockerId = userId,
				BlockedId = request.BlockedId
			};

			bool isFollowCancel = await _followService.CancelAllFollowingsByFollowingIdAsync(request.BlockedId);
			bool isFriendshipCancel =  await _friendshipService.RemoveFriendsByUserIdAsync(request.BlockedId);
			if (isFollowCancel && isFriendshipCancel) await _blockService.BlockUserAsync(block);
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
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			var blockList = await _blockService.GetBlockListAsync(userId);
			if (blockList is null) return BadRequest();
			return Ok(blockList);
		}
	}
}
