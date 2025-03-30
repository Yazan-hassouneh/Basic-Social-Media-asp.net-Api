using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class PostReactionController(IPostReactionService postReactionService, IValidator<AddPostReactionDto> addPostReactionDtoValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IPostReactionService _postReactionService = postReactionService;
		private readonly IValidator<AddPostReactionDto> _addPostReactionDtoValidator = addPostReactionDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;

		[HttpGet]
		[Route("getAllByPostId/{postId}")]
		public async Task<IActionResult> GetAllReactionsByPostId(int postId)
		{
			IEnumerable<GetReactionDto> reactions = await _postReactionService.GetPostReactionsByPostIdAsync(postId);
			if (reactions == null) return BadRequest();
			return Ok(reactions);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewReaction([FromBody] AddPostReactionDto postReactionDto)
		{
			var isUserOwner = await _authorizationService.AuthorizeAsync(User, postReactionDto.UserId, PoliciesSettings.Ownership);
			if (!isUserOwner.Succeeded) return Forbid();

			var result = await _addPostReactionDtoValidator.ValidateAsync(postReactionDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddPostReactionDto reaction = await _postReactionService.CreatePostReactionAsync(postReactionDto);
			return Ok(reaction);
		}

		[HttpDelete]
		[Route("delete/{reactionId}")]
		public async Task<IActionResult> DeletePostReaction(int reactionId)
		{
			string? userId = await _postReactionService.GetUserId(reactionId);
			if (userId is null) return NotFound("Post not found or update failed.");

			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDeletePost = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.CanDeletePost);
			if (!authorizationCanDeletePost.Succeeded && !authorizationOwnership.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			bool isDeleted = await _postReactionService.DeletePostReactionAsync(reactionId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
