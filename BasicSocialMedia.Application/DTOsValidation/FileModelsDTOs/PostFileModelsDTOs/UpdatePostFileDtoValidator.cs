using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using BasicSocialMedia.Core.Consts;

namespace BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.PostFileModelsDTOs
{
	public class UpdatePostFileDtoValidator : AbstractValidator<UpdatePostFileDto>
	{
		public UpdatePostFileDtoValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			Include(new BaseUserIdDtoValidation(userManager));
			Include(new BasePostIdDtoValidation(context));
			Include(new BaseIFormFile());

			RuleFor(x => x)
				.Custom((user, context) =>
				{
					if (user.MediaPaths is null && user.Files is null)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
					if (user.MediaPaths!.Count == 0 && user.Files!.Count == 0)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
				});
		}
	}
}
