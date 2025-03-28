using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation
{
	public class SendFriendRequestDtoValidator : AbstractValidator<SendFriendRequestDto>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SendFriendRequestDtoValidator(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;

			RuleFor(x => x.SenderId)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage("Sender does not exist.")
				.Must((request, senderId) => IsValidSender(request.SenderId, request.ReceiverId))
				.WithMessage("Sender must be the current user, and receiver must be different from the current user.");

			RuleFor(x => x.ReceiverId)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage("Receiver does not exist.")
				.Must((request, receiverId) => IsValidReceiver(request.ReceiverId, request.SenderId))
				.WithMessage("Receiver must not be the current user and must be different from the sender.");

		}

		private async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(userId);
			return user != null;
		}
		private bool IsValidSender(string senderId, string receiverId)
		{
			var currentUserId = GetCurrentUserId();
			// Ensure the sender is the current user, the receiver is not the current user, and they are not the same person
			return senderId == currentUserId && receiverId != currentUserId && receiverId != senderId;
		}

		private bool IsValidReceiver(string receiverId, string senderId)
		{
			var currentUserId = GetCurrentUserId();
			// Ensure the sender is the current user, the receiver is not the current user, and they are not the same person
			return receiverId != currentUserId && senderId == currentUserId && receiverId != senderId;
		}
		private string? GetCurrentUserId()
		{
			// Here, we assume the current user is stored in claims
			return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
