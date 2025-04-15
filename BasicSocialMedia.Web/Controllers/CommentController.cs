using BasicSocialMedia.Application.DTOsValidation.PostDtosValidation;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Application.Services.ValidationServices;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.ValidationServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class CommentController(ICommentService commentService, IValidator<AddCommentDto> addCommentDtoValidator, IValidator<UpdateCommentDto> updateCommentDtoValidator, IAuthorizationService authorizationService, IFileValidationResult fileValidationResult) : ControllerBase
	{
		private readonly ICommentService _commentService = commentService;
		private readonly IValidator<AddCommentDto> _addCommentDtoValidator = addCommentDtoValidator;
		private readonly IValidator<UpdateCommentDto> _updateCommentDtoValidator = updateCommentDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly IFileValidationResult _fileValidationResult = fileValidationResult;


		[HttpGet]
		[Route("getById/{CommentId}")]
		public async Task<IActionResult> GetCommentById(int CommentId)
		{
			GetCommentDto Comment = await _commentService.GetCommentByIdAsync(CommentId);
			if (Comment == null) return NotFound();

			var isUserBlocked = await _authorizationService.AuthorizeAsync(User, Comment.User.Id, PoliciesSettings.IsUserBlocked);
			if ( !isUserBlocked.Succeeded) return Forbid(); // User is not allowed to see this post

			return Ok(Comment);
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllCommentsByUserId(string userId)
		{
			var isUserOwner = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!isUserOwner.Succeeded) return Forbid(); 

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

			var filteredComments = new List<GetCommentDto>();

			foreach (var comment in Comments)
			{
				var isUserBlocked = await _authorizationService.AuthorizeAsync(User, comment.User.Id, PoliciesSettings.IsUserBlocked);
				//var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, PoliciesSettings.PostVisibilityPolicy);
				if (isUserBlocked.Succeeded) filteredComments.Add(comment);
			}

			return Ok(filteredComments);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewComment([FromForm] AddCommentDto CommentDto)
		{
			var dtoResult = await _addCommentDtoValidator.ValidateAsync(CommentDto);
			var fileResult = await _fileValidationResult.ValidateFiles(CommentDto.Files);

			if (!dtoResult.IsValid) return BadRequest(dtoResult.Errors);
			if (string.IsNullOrEmpty(CommentDto.Content) && !fileResult.Any()) return BadRequest(dtoResult.Errors);

			var isUserOwner = await _authorizationService.AuthorizeAsync(User, CommentDto.UserId, PoliciesSettings.Ownership);
			if (!isUserOwner.Succeeded) return Forbid();

			AddCommentDto Comment = await _commentService.CreateCommentAsync(CommentDto);
			return Ok(Comment);
		}

		[HttpPut]
		[Route("update/{commentId}")]
		public async Task<IActionResult> UpdateComment([FromForm] UpdateCommentDto CommentDto)
		{
			var dtoResult = await _updateCommentDtoValidator.ValidateAsync(CommentDto);
			var fileResult = await _fileValidationResult.ValidateFiles(CommentDto.Files);

			if (!dtoResult.IsValid) return BadRequest(dtoResult.Errors);
			if (string.IsNullOrEmpty(CommentDto.Content) && !fileResult.Any() && CommentDto.MediaPaths!.Count == 0) return BadRequest(dtoResult.Errors);

			var isUserOwner = await _authorizationService.AuthorizeAsync(User, CommentDto.UserId, PoliciesSettings.Ownership);
			if (!isUserOwner.Succeeded) return Forbid();

			UpdateCommentDto? Comment = await _commentService.UpdateCommentAsync(CommentDto);
			if (Comment is null) return NotFound("Comment not found or update failed.");
			return Ok(Comment);
		}

		[HttpDelete]
		[Route("delete/{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			string? userId = await _commentService.GetUserId(commentId);
			if (userId is null) return NotFound("Post not found or update failed.");

			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDeletePost = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.CanDeletePost);
			if (!authorizationCanDeletePost.Succeeded && !authorizationOwnership.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			bool isDeleted = await _commentService.DeleteCommentAsync(commentId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
