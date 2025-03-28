using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation
{
	public class SendFollowRequestDtoValidator : AbstractValidator<SendFollowRequestDto>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SendFollowRequestDtoValidator(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;

			RuleFor(x => x.FollowerId)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage(ValidationSettings.GeneralErrorMessage)
				.Must((request, followerId) => IsValidFollower(request.FollowerId, request.FollowingId))
				.WithMessage("Follower cannot be the same as the current user."); 

			RuleFor(x => x.FollowingId)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage(ValidationSettings.GeneralErrorMessage)
				.Must((request, followingId) => IsValidFollowing(request.FollowingId, request.FollowerId))
				.WithMessage("Following cannot be the same as the current user.");
		}

		private async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(userId);
			return user != null;
		}
		private bool IsValidFollower(string followerId, string followingId)
		{
			var currentUserId = GetCurrentUserId();
			// Ensure the follower is the current user, the following is not the current user, and they are not the same person
			return followerId == currentUserId && followingId != currentUserId && followingId != followerId;
		}

		private bool IsValidFollowing(string followingId, string followerId)
		{
			var currentUserId = GetCurrentUserId();
			// Ensure the follower is the current user, the following is not the current user, and they are not the same person
			return followingId != currentUserId && followerId == currentUserId && followingId != followerId;
		}

		private string? GetCurrentUserId()
		{
			// Here, we assume the current user is stored in claims
			return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
