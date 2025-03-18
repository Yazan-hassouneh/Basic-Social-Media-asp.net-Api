using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseUserIdDtoValidation : AbstractValidator<IUserIdDto>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public BaseUserIdDtoValidation(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			RuleFor(x => x.UserId)
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
