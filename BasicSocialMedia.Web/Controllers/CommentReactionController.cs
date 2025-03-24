using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentReactionController(ICommentReactionService commentReactionService, IValidator<AddCommentReactionDto> addCommentReactionDtoValidator) : ControllerBase
	{
		private readonly ICommentReactionService _commentReactionService = commentReactionService;
		private readonly IValidator<AddCommentReactionDto> _addCommentReactionDtoValidator = addCommentReactionDtoValidator;

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
			var result = await _addCommentReactionDtoValidator.ValidateAsync(commentReactionDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddCommentReactionDto reaction = await _commentReactionService.CreateCommentReactionAsync(commentReactionDto);
			return Ok(reaction);
		}

		[HttpDelete]
		[Route("delete/{reactionId}")]
		public async Task<IActionResult> DeleteComment(int reactionId)
		{
			bool isDeleted = await _commentReactionService.DeleteCommentReaction(reactionId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
