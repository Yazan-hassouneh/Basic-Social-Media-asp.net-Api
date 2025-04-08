using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.ProfileImage
{
	public class UpdateProfileImageValidator : AbstractValidator<UpdateProfileImageDto>
	{
		public UpdateProfileImageValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseUserIdDtoValidation(userManager));

			RuleFor(x => x.Image).SetValidator(new BaseSingleFileValidator() as IValidator<IFormFile?>);
			RuleFor(x => x)
				.Custom((user, context) =>
				{
					if (string.IsNullOrEmpty(user.ImagePath) && user.Image == null)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
				});
		}
	}
}
