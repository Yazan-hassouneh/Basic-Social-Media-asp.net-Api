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
	public class FollowController(IFollowService followService, IValidator<SendFollowRequestDto> sendFollowRequestValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IFollowService _followService = followService;
		private readonly IValidator<SendFollowRequestDto> _sendFollowRequestValidator = sendFollowRequestValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;


		[HttpGet]
		[Route("getAllFollowers/{userId}")]
		public async Task<IActionResult> GetAllFollowersAsync(string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			IEnumerable<GetFollowerDto> followers = await _followService.GetAllFollowersAsync(userId);
			if (followers == null) return BadRequest();
			return Ok(followers);
		}

		[HttpGet]
		[Route("getAllFollowings/{userId}")]
		public async Task<IActionResult> GetAllFollowingsAsync(string userId)
		{
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			IEnumerable<GetFollowingDto> followings = await _followService.GetAllFollowingsAsync(userId);
			if (followings == null) return BadRequest();
			return Ok(followings);
		}

		[HttpPost]
		[Route("follow")]
		public async Task<IActionResult> FollowAsync([FromBody] SendFollowRequestDto requestDto)
		{
			var result = await _sendFollowRequestValidator.ValidateAsync(requestDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			var isCurrentUserTheOwner = await _authorizationService.AuthorizeAsync(User, requestDto.FollowerId, PoliciesSettings.Ownership);
			var isUserBlocked = await _authorizationService.AuthorizeAsync(User, requestDto.FollowingId, PoliciesSettings.IsUserBlocked);
			if(!isCurrentUserTheOwner.Succeeded && !isUserBlocked.Succeeded) return Forbid();

			bool followRequest = await _followService.FollowAsync(requestDto);
			return Ok(followRequest);
		}

		[HttpDelete]
		[Route("cancelFollowing/{followId}")]
		public async Task<IActionResult> CancelFollowingAsync(int followId)
		{
			SendFollowRequestDto? followRequest = await _followService.GetByIdAsync(followId);
			if (followRequest == null) return NotFound();

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, followRequest.FollowerId, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, followRequest.FollowingId, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither FollowerId nor FollowingId

			bool result = await _followService.CancelFollowingAsync(followId);
			if (!result) return NotFound(new { Message = "Something went wrong" });
			return Ok(new { Message = "following canceled successfully" });
		}
	}
}
