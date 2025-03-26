using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation
{
	public class SendFollowRequestDtoValidator : AbstractValidator<SendFollowRequestDto>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public SendFollowRequestDtoValidator(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;

			RuleFor(x => x.FollowerId)
				.NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(UserExists).WithMessage(ValidationSettings.GeneralErrorMessage);

			RuleFor(x => x.FollowingId)
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
