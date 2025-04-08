using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.PostDtosValidation
{
	public class AddPostDtoValidator : AbstractValidator<AddPostDto>
	{
		public AddPostDtoValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseCreateFileValidator());
			Include(new BaseAudienceDtoValidation());
			Include(new BaseUserIdDtoValidation(userManager));
		}
	}
}
