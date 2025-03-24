using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostReactionController(IPostReactionService postReactionService, IValidator<AddPostReactionDto> addPostReactionDtoValidator) : ControllerBase
	{
		private readonly IPostReactionService _postReactionService = postReactionService;
		private readonly IValidator<AddPostReactionDto> _addPostReactionDtoValidator = addPostReactionDtoValidator;

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
			var result = await _addPostReactionDtoValidator.ValidateAsync(postReactionDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddPostReactionDto reaction = await _postReactionService.CreatePostReactionAsync(postReactionDto);
			return Ok(reaction);
		}

		[HttpDelete]
		[Route("delete/{reactionId}")]
		public async Task<IActionResult> DeletePost(int reactionId)
		{
			bool isDeleted = await _postReactionService.DeletePostReactionAsync(reactionId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
