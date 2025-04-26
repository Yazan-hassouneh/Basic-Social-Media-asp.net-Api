using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.Consts;
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
	public class PostController(IPostService postService, IValidator<AddPostDto> addPostDtoValidator, IValidator<UpdatePostDto> updatePostDtoValidator, IAuthorizationService authorizationService, IFileValidationResult fileValidationResult) : ControllerBase
	{
		private readonly IPostService _postService = postService;
		private readonly IValidator<AddPostDto> _addPostDtoValidator = addPostDtoValidator;
		private readonly IValidator<UpdatePostDto> _updatePostDtoValidator = updatePostDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly IFileValidationResult _fileValidationResult = fileValidationResult;


		[HttpGet]
		[Route("getById/{postId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetPostById(int postId)
		{
			GetPostDto? post = await _postService.GetPostByIdAsync(postId);
			if (post == null) return NotFound();

			var isUserBlocked = await _authorizationService.AuthorizeAsync(User, post.UserId, PoliciesSettings.IsUserBlocked);
			var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, PoliciesSettings.PostVisibilityPolicy);
			if (!authorizationResult.Succeeded && !isUserBlocked.Succeeded) return Forbid(); // User is not allowed to see this post
			return Ok(post);
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllPostsByUserId([FromRoute] string userId)
		{
			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserIdAsync(userId);
			if (posts == null) return BadRequest();

			var filteredPosts = new List<GetPostDto>();

			foreach (var post in posts)
			{
				var isUserBlocked = await _authorizationService.AuthorizeAsync(User, post.UserId, PoliciesSettings.IsUserBlocked);
				var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, PoliciesSettings.PostVisibilityPolicy);
				if (authorizationResult.Succeeded && isUserBlocked.Succeeded) filteredPosts.Add(post);
			}

			return Ok(filteredPosts);
		}		

		[HttpGet]
		[Route("getAllByUserFriends")]
		public async Task<IActionResult> GetAllPostsByUserFriends()
		{
			var userId = User.GetUserId();
			if (userId == null) return Unauthorized();

			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserFriendsAsync(userId);
			if (posts == null) return BadRequest();
			if (!posts.Any()) return NoContent();
			return Ok(posts);
		}		

		[HttpGet]
		[Route("getAllByUserFollowings")]
		public async Task<IActionResult> GetAllPostsByUserFollowings()
		{
			var userId = User.GetUserId();
			if (userId == null) return Unauthorized();

			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserFollowingsAsync(userId);
			if (posts == null) return BadRequest();
			if (!posts.Any()) return NoContent();
			return Ok(posts);
		}		

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewPost([FromForm]AddPostDto postDto)
		{
			var dtoResult = await _addPostDtoValidator.ValidateAsync(postDto);
			var fileResult = await _fileValidationResult.ValidateFiles(postDto.Files);

			if (!dtoResult.IsValid) return BadRequest(dtoResult.Errors);
			if (string.IsNullOrEmpty(postDto.Content) && !fileResult.Any()) return BadRequest(dtoResult.Errors);

			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, postDto.UserId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();

			AddPostDto post = await _postService.CreatePostAsync(postDto);
			return Ok(post);
		}

		[HttpPut]
		[Route("update")]
		public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDto postDto)
		{
			var dtoResult = await _updatePostDtoValidator.ValidateAsync(postDto);
			var fileResult = await _fileValidationResult.ValidateFiles(postDto.Files);

			if (!dtoResult.IsValid) return BadRequest(dtoResult.Errors);
			if (string.IsNullOrEmpty(postDto.Content) && !fileResult.Any() && postDto?.MediaPaths!.Count == 0) return BadRequest(dtoResult.Errors);

			string? userId = await _postService.GetUserId(postDto!.Id);
			if (userId is null) return NotFound("Post not found or update failed.");

			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDeletePost = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.CanEditPost);
			if (!authorizationCanDeletePost.Succeeded && !authorizationOwnership.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			UpdatePostDto? UpdatedPost = await _postService.UpdatePostAsync(postDto);
			if (UpdatedPost is null) return NotFound("Post not found or update failed.");
			return Ok(UpdatedPost);
		}

		[HttpDelete]
		[Route("delete/{postId}")]
		public async Task<IActionResult> DeletePost(int postId)
		{
			string? userId = await _postService.GetUserId(postId);
			if (userId is null) return NotFound("Post not found or update failed.");

			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDeletePost = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.CanDeletePost);
			if (!authorizationCanDeletePost.Succeeded && !authorizationOwnership.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			bool isDeleted = await _postService.DeletePostAsync(postId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}

	}
}
