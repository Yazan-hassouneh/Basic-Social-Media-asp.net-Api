using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class ProfileImageController(IProfileImageModelService profileImageModelService, IAuthorizationService authorizationService, IValidator<AddProfileImageDto> addProfileImageValidator) : ControllerBase
	{
		private readonly IProfileImageModelService _profileImageModelService = profileImageModelService;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly IValidator<AddProfileImageDto> _addProfileImageValidator = addProfileImageValidator;


		[HttpGet("GetAllImagesByUserId/{userId}")]
		public async Task<IActionResult> GetAllImagesByUserId([FromRoute]string userId)
		{
			IEnumerable<string> images = await _profileImageModelService.GetAllImagesByUserIdAsync(userId);
			if (images == null) return BadRequest();
			return Ok(images);
		}

		[HttpPost("AddProfileImage")]
		public async Task<IActionResult> AddProfileImage([FromForm] AddProfileImageDto addProfileImageDto)
		{
			var ValidationResult = await _addProfileImageValidator.ValidateAsync(addProfileImageDto);
			if (!ValidationResult.IsValid) return BadRequest(ValidationResult.Errors);

			var result = await _profileImageModelService.AddProfileImageAsync(addProfileImageDto);
			if (result) return Ok();
			return BadRequest();
		}

		[HttpPut("ChangeProfileImage")]
		public async Task<IActionResult> ChangeProfileImage([FromForm] AddProfileImageDto addProfileImageDto)
		{
			var validationResult = await _addProfileImageValidator.ValidateAsync(addProfileImageDto);
			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, addProfileImageDto.UserId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();// Return forbidden if the user does not meet the policy requirements

			var result = await _profileImageModelService.UpdateMessageFileAsync(addProfileImageDto);
			if (result) return Ok();
			return BadRequest();
		}

		[HttpDelete("DeleteProfileImageByImageId/{profileImageId}")]
		[Authorize(Policy = PoliciesSettings.allowSuperAdminAdminModeratorPolicy)]
		public async Task<IActionResult> DeleteProfileImageByImageId([FromRoute]int profileImageId)
		{
			string? userId = await _profileImageModelService.GetUserId(profileImageId);
			if (string.IsNullOrEmpty(userId)) return NotFound();
			// Explicitly authorize with resource
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			var authorizationCanDelete = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.allowSuperAdminAdminModeratorPolicy);
			if (!authorizationOwnership.Succeeded && !authorizationCanDelete.Succeeded)
			{
				return Forbid(); // Return forbidden if the user does not meet the policy requirements
			}

			var result = await _profileImageModelService.DeleteProfileImageByImageIdAsync(profileImageId);
			if (result) return Ok();
			return BadRequest();

		}



	}
}
