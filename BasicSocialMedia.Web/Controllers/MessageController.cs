using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class MessageController(IMessagesServices messagesServices, IValidator<AddMessageDto> addMessageDtoValidator, IAuthorizationService authorizationService, IValidator<UpdateMessageDto> updateMessageDtoValidator) : ControllerBase
	{
		private readonly IMessagesServices _messagesServices = messagesServices;
		private readonly IValidator<AddMessageDto> _addMessageDtoValidator = addMessageDtoValidator;
		private readonly IValidator<UpdateMessageDto> _updateMessageDtoValidator = updateMessageDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;

		[HttpPost("AddMessage")]
		public async Task<IActionResult> AddMessageAsync([FromForm] AddMessageDto addMessageDto)
		{
			ValidationResult validationResult = await _addMessageDtoValidator.ValidateAsync(addMessageDto);

			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, addMessageDto.ReceiverId, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, addMessageDto.SenderId, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither User1 nor User2    

			var userId = User.GetUserId();
			if (userId == null) return Unauthorized(); // Ensure User.GetUserId() is not null  
			addMessageDto.SenderId = userId;

			var result = await _messagesServices.CreateMessageAsync(addMessageDto);
			if (result) return Ok();
			return BadRequest();
		}		

		[HttpPut("updateMessage")]
		public async Task<IActionResult> UpdateMessageAsync([FromForm] UpdateMessageDto updateMessageDto)
		{
			ValidationResult validationResult = await _updateMessageDtoValidator.ValidateAsync(updateMessageDto);
			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

			MessageExistDto? messageExistDto = await _messagesServices.GetMessageByIdAsync(updateMessageDto.Id);
			if (messageExistDto == null) return BadRequest();

			bool isSender = updateMessageDto.UserId == messageExistDto.SenderId;
			var userOwnership = await _authorizationService.AuthorizeAsync(User, updateMessageDto.UserId, PoliciesSettings.Ownership);
			if (!userOwnership.Succeeded || !isSender) return Forbid(); 

			var result = await _messagesServices.UpdateMessageAsync(updateMessageDto);
			if (result) return Ok();
			return BadRequest();
		}

		[HttpDelete("delete/{messageId}")]
		public async Task<IActionResult> DeleteMessageAsync(int messageId)
		{
			var message = await _messagesServices.GetMessageByIdAsync(messageId);
			if (message == null) return NotFound();

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, message.SenderId, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded ) return Forbid(); 

			var result = await _messagesServices.DeleteMessageAsync(messageId);
			if (result) return Ok();
			return BadRequest();
		}
	}
}
