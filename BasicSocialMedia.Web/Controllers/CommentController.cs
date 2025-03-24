using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController(ICommentService commentService, IValidator<AddCommentDto> addCommentDtoValidator, IValidator<UpdateCommentDto> updateCommentDtoValidator) : ControllerBase
	{
		private readonly ICommentService _commentService = commentService;
		private readonly IValidator<AddCommentDto> _addCommentDtoValidator = addCommentDtoValidator;
		private readonly IValidator<UpdateCommentDto> _updateCommentDtoValidator = updateCommentDtoValidator;

		[HttpGet]
		[Route("getById/{CommentId}")]
		public async Task<IActionResult> GetCommentById(int CommentId)
		{
			GetCommentDto Comment = await _commentService.GetCommentByIdAsync(CommentId);
			if (Comment == null) return NotFound();
			return Ok(Comment);
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllCommentsByUserId(string userId)
		{
			IEnumerable<GetCommentDto> Comments = await _commentService.GetCommentsByUserIdAsync(userId);
			if (Comments == null) return BadRequest();
			return Ok(Comments);
		}		
		
		[HttpGet]
		[Route("getAllByPostId/{postId}")]
		public async Task<IActionResult> GetAllCommentsByPostId(int postId)
		{
			IEnumerable<GetCommentDto> Comments = await _commentService.GetCommentsByPostIdAsync(postId);
			if (Comments == null) return BadRequest();
			return Ok(Comments);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewComment([FromBody] AddCommentDto CommentDto)
		{
			var result = await _addCommentDtoValidator.ValidateAsync(CommentDto);
			if (!result.IsValid) return BadRequest(result.Errors);
			AddCommentDto Comment = await _commentService.CreateCommentAsync(CommentDto);
			return Ok(Comment);
		}

		[HttpPut]
		[Route("update/{commentId}")]
		public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto CommentDto)
		{
			var result = await _updateCommentDtoValidator.ValidateAsync(CommentDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			UpdateCommentDto? Comment = await _commentService.UpdateCommentAsync(CommentDto);
			if (Comment is null) return NotFound("Comment not found or update failed.");
			return Ok(Comment);
		}

		[HttpDelete]
		[Route("delete/{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			bool isDeleted = await _commentService.DeleteCommentAsync(commentId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
