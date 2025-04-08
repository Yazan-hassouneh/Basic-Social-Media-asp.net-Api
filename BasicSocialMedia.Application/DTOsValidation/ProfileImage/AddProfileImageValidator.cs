using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using FluentValidation;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.ProfileImage
{
	public class AddProfileImageValidator : AbstractValidator<AddProfileImageDto>
	{
		public AddProfileImageValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseUserIdDtoValidation(userManager));

			RuleFor(x => x.Image)
				.SetValidator(new BaseSingleFileValidator());
		}
	}
}
