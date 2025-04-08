using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Infrastructure.Data;

namespace BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.PostFileModelsDTOs
{
	public class AddPostFileModelDtoValidator : AbstractValidator<AddPostFileDto>
	{
		public AddPostFileModelDtoValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			Include(new BaseUserIdDtoValidation(userManager));
			Include(new BasePostIdDtoValidation(context));
			Include(new BaseIFormFile());

		}
	}
}
