using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation
{
	public class SendFriendRequestDtoValidator : AbstractValidator<SendFriendRequestDto>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public SendFriendRequestDtoValidator(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;

			RuleFor(x => x.UserId1)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage(ValidationSettings.GeneralErrorMessage);

			RuleFor(x => x.UserId2)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}

		private async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(userId);
			return user != null;
		}
	}
}
