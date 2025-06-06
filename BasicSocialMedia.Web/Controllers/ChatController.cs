﻿using BasicSocialMedia.Application.BackgroundJobs;
using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class ChatController(IChatServices chatServices, IValidator<AddChatDto> addChatDtoValidator, IAuthorizationService authorizationService) : ControllerBase
	{
		private readonly IChatServices _chatServices = chatServices;
		private readonly IValidator<AddChatDto> _addChatDtoValidator = addChatDtoValidator;
		private readonly IAuthorizationService _authorizationService = authorizationService;


		[HttpGet]
		[Route("getById/{chatId}")]
		public async Task<IActionResult> GetChatById(int chatId)
		{
			var userId = User.GetUserId(); // Extract the string value from the claim  
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			GetChatDto? chat = await _chatServices.GetChatByIdAsync(chatId, userId);
			if (chat == null) return NotFound();

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, chat.User1Id, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, chat.User2Id, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither User1 nor User2  

			return Ok(chat);
		}		
		
		[HttpGet]
		[Route("getChatFile/{chatId}")]
		[Authorize]
		public async Task<IActionResult> GetChatFilesById(int chatId)
		{
			var userId = User.GetUserId(); // Extract the string value from the claim  
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var usersId = await _chatServices.GetUsersId(chatId).ConfigureAwait(false);
			if (usersId is null) return Forbid();

			if (!usersId.TryGetValue("user1Id", out var user1Id) || !usersId.TryGetValue("user2Id", out var user2Id)) return Forbid();

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, user1Id, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, user2Id, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither User1 nor User2  

			IEnumerable<string>? files = await _chatServices.GetFilesByChatIdAsync(chatId);
			return files is not null ? Ok(files) : NotFound();
		}

		[HttpGet]
		[Route("getAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllChatsByUserId(string userId)
		{
			var userOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!userOwnership.Succeeded) return Forbid(); 

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

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, chatDto.SenderId, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, chatDto.ReceiverId, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither User1 nor User2  

			string? userId = User.GetUserId(); // Marked as nullable to handle potential null values  
			if (userId is null) return Unauthorized();
			chatDto.SenderId = userId;

			AddChatDto chat = await _chatServices.CreateChatAsync(chatDto);
			return Ok(chat);
		}

		[HttpDelete]
		[Route("delete/{chatId}")]
		public async Task<IActionResult> DeleteChat(int chatId)
		{
			var userId = User.GetUserId(); // Marked as nullable to handle potential null values  
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			GetChatDto? chat = await _chatServices.GetChatByIdAsync(chatId, userId);
			if (chat == null) return BadRequest();

			var user1Ownership = await _authorizationService.AuthorizeAsync(User, chat.User1Id, PoliciesSettings.Ownership);
			var user2Ownership = await _authorizationService.AuthorizeAsync(User, chat.User2Id, PoliciesSettings.Ownership);
			if (!user1Ownership.Succeeded && !user2Ownership.Succeeded) return Forbid(); // User is neither User1 nor User2  

			bool isDeleted = await _chatServices.SoftDeleteChatAsync(chatId, userId);
			if (!isDeleted) return BadRequest("something went wrong!");
			BackgroundJob.Enqueue<ChatBackgroundJobs>(x => x.HardDeleteChat(chatId));
			return Ok();
		}
	}
}
