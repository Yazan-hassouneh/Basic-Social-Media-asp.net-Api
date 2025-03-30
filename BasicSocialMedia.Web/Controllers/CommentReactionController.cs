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
	public class CommentReactionController(ICommentReactionService commentReactionService, IValidator<AddCommentReactionDto> addCommentReactionDtoValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly ICommentReactionService _commentReactionService = commentReactionService;
		private readonly IValidator<AddCommentReactionDto> _addCommentReactionDtoValidator = addCommentReactionDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;

		[HttpGet]
		[Route("getAllByCommentId/{commentId}")]
		public async Task<IActionResult> GetAllReactionsByCommentId(int commentId)
		{
			IEnumerable<GetReactionDto> reactions = await _commentReactionService.GetCommentReactionsByCommentIdAsync(commentId);
			if (reactions == null) return BadRequest();
			return Ok(reactions);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewReaction([FromBody] AddCommentReactionDto commentReactionDto)
		{
			var isUserOwner = await _authorizationService.AuthorizeAsync(User, commentReactionDto.UserId, PoliciesSettings.Ownership);
			if (!isUserOwner.Succeeded) return Forbid();

			var result = await _addCommentReactionDtoValidator.ValidateAsync(commentReactionDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddCommentReactionDto reaction = await _commentReactionService.CreateCommentReactionAsync(commentReactionDto);
			return Ok(reaction);
		}

		[HttpDelete]
		[Route("delete/{reactionId}")]
		public async Task<IActionResult> DeleteCommentReaction(int reactionId)
		{
			string? userId = await _commentReactionService.GetUserId(reactionId);
			if (userId is null) return NotFound("Post not found or update failed.");
			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDeletePost = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.CanDeletePost);
			if (!authorizationCanDeletePost.Succeeded && !authorizationOwnership.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			bool isDeleted = await _commentReactionService.DeleteCommentReaction(reactionId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
