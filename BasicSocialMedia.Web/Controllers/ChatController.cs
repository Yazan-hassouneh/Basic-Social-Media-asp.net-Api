using BasicSocialMedia.Application.DTOsValidation.ChatDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.PostDtosValidation;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController(IChatServices chatServices, IValidator<AddChatDto> addChatDtoValidator) : ControllerBase
	{
		private readonly IChatServices _chatServices = chatServices;
		private readonly IValidator<AddChatDto> _addChatDtoValidator = addChatDtoValidator;

		[HttpGet]
		[Route("getById/{chatId}")]
		public async Task<IActionResult> GetChatById(int chatId)
		{
			GetChatDto? chat = await _chatServices.GetChatByIdAsync(chatId);
			if (chat == null) return NotFound();
			return Ok(chat);
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllChatsByUserId(string userId)
		{
			IEnumerable<GetChatDto>? chats = await _chatServices.GetChatsByUserIdAsync(userId);
			if (chats == null) return BadRequest();
			return Ok(chats);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateNewChat([FromBody] AddChatDto chatDto)
		{
			var result = await _addChatDtoValidator.ValidateAsync(chatDto);
			if (!result.IsValid) return BadRequest(result.Errors);

			AddChatDto chat = await _chatServices.CreateChatAsync(chatDto);
			return Ok(chat);
		}

		[HttpDelete]
		[Route("delete/{chatId}")]
		public async Task<IActionResult> DeleteChat(int chatId)
		{
			bool isDeleted = await _chatServices.DeleteChatAsync(chatId);
			if (!isDeleted) return BadRequest("something went wrong!");
			return Ok();
		}
	}
}
