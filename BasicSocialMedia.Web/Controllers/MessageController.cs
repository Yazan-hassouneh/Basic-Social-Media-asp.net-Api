using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController(IMessageFileModelService messageFileModelService) : ControllerBase
	{
		private readonly IMessageFileModelService _messageFileModelService = messageFileModelService;

		[HttpGet("GetAllFilesByChatId/{chatId}")]
		public async Task<IActionResult> GetAllFilesByChatId(int chatId)
		{
			var result = await _messageFileModelService.GetAllFilesByChatIdAsync(chatId);
			return Ok(result);
		}

		[HttpGet("GetAllFilesByMessageId/{messageId}")]
		public async Task<IActionResult> GetAllFilesByMessageId(int messageId)
		{
			var result = await _messageFileModelService.GetAllFilesByMessageIdAsync(messageId);
			return Ok(result);
		}

		[HttpGet("GetAllFilesByUserId/{userId}")]
		public async Task<IActionResult> GetAllFilesByUserId(string userId)
		{
			var result = await _messageFileModelService.GetAllFilesByUserIdAsync(userId);
			return Ok(result);
		}

		[HttpPost("AddMessageFile")]
		public async Task<IActionResult> AddMessageFile([FromForm] AddMessageFileDto addMessageFilesDto)
		{
			var result = await _messageFileModelService.AddMessageFileAsync(addMessageFilesDto);
			if (result) return Ok();
			return BadRequest();
		}
	}
}
