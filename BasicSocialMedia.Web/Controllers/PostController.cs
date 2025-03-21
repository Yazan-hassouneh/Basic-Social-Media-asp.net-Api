using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostController(IPostService postService, IValidator<AddPostDto> addPostDtoValidator, IValidator<UpdatePostDto> updatePostDtoValidator) : ControllerBase
	{
		private readonly IPostService _postService = postService;
		private readonly IValidator<AddPostDto> _addPostDtoValidator = addPostDtoValidator;
		private readonly IValidator<UpdatePostDto> _updatePostDtoValidator = updatePostDtoValidator;

		[HttpGet]
		[Route("getById/{postId}")]
		public async Task<IActionResult> GetPostById(int postId)
		{
			GetPostDto post = await _postService.GetPostByIdAsync(postId);
			if (post == null) return NotFound();
			return Ok(post);
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllPostsByUserId(string userId)
		{
			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserIdAsync(userId);
			if (posts == null) return BadRequest();
			return Ok(posts);
		}		

		[HttpGet]
		[Route("getAllByUserFriends/{userId}")]
		public async Task<IActionResult> GetAllPostsByUserFriends(string userId)
		{
			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserFriendsAsync(userId);
			if (posts == null) return BadRequest();
			return Ok(posts);
		}		

		[HttpGet]
		[Route("getAllByUserFollowings/{userId}")]
		public async Task<IActionResult> GetAllPostsByUserFollowings(string userId)
		{
			IEnumerable<GetPostDto> posts = await _postService.GetPostsByUserFollowingsAsync(userId);
			if (posts == null) return BadRequest();
			return Ok(posts);
		}		

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewPost([FromBody] AddPostDto postDto)
		{
			var result = await _addPostDtoValidator.ValidateAsync(postDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddPostDto post = await _postService.CreatePostAsync(postDto);
			return Ok(post);
		}

		[HttpPut]
		[Route("update/{postId}")]
		public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postDto)
		{
			var result = await _updatePostDtoValidator.ValidateAsync(postDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			UpdatePostDto? post = await _postService.UpdatePostAsync(postDto);
			if (post is null) return NotFound("Post not found or update failed.");
			return Ok(post);
		}

		[HttpDelete]
		[Route("delete/{postId}")]
		public async Task<IActionResult> DeletePost(int postId)
		{
			bool isDeleted = await _postService.DeletePostAsync(postId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}

	}
}
