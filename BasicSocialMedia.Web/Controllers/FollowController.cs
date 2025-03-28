using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FollowController(IFollowService followService, IValidator<SendFollowRequestDto> sendFollowRequestValidator, IBlockService blockService) : ControllerBase
	{
		private readonly IFollowService _followService = followService;
		private readonly IBlockService _blockService = blockService;
		private readonly IValidator<SendFollowRequestDto> _sendFollowRequestValidator = sendFollowRequestValidator;

		[HttpGet]
		[Route("getAllFollowers/{userId}")]
		public async Task<IActionResult> GetAllFollowersAsync(string userId)
		{
			string? claimUserId = CheckUserId();
			if (string.IsNullOrEmpty(userId) || claimUserId != userId) return Unauthorized();	
			IEnumerable<GetFollowerDto> followers = await _followService.GetAllFollowersAsync(userId);
			if (followers == null) return BadRequest();
			return Ok(followers);
		}

		[HttpGet]
		[Route("getAllFollowings")]
		public async Task<IActionResult> GetAllFollowingsAsync(string userId)
		{
			string? claimUserId = CheckUserId();
			if (string.IsNullOrEmpty(userId) || claimUserId != userId) return Unauthorized();
			IEnumerable<GetFollowingDto> followings = await _followService.GetAllFollowingsAsync(userId);
			if (followings == null) return BadRequest();
			return Ok(followings);
		}

		[HttpPost]
		[Route("follow")]
		public async Task<IActionResult> FollowAsync([FromBody] SendFollowRequestDto requestDto)
		{
			bool isBlocked = await _blockService.IsUsersBlocked(requestDto.FollowerId, requestDto.FollowingId);
			if (isBlocked) return BadRequest();
			var result = _sendFollowRequestValidator.Validate(requestDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			bool followRequest = await _followService.FollowAsync(requestDto);
			return Ok(followRequest);
		}

		[HttpDelete]
		[Route("cancelFollowing/{followId}")]
		public async Task<IActionResult> RejectRequestAsync(int followId)
		{
			bool result = await _followService.CancelFollowingAsync(followId);
			if (!result) return NotFound(new { Message = "Something went wrong" });
			return Ok(new { Message = "following canceled successfully" });
		}

		private string? CheckUserId() 
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
