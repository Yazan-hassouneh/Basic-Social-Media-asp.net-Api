using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.MessageFileModelsDTOs
{
	public class UpdateMessageFileDtoValidator : AbstractValidator<UpdateMessageFileDto>
	{
		public UpdateMessageFileDtoValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			Include(new BaseUserIdDtoValidation(userManager));
			Include(new BaseMessageIdDtoValidator(context));
			Include(new BaseChatIdDtoValidator(context));
			Include(new BaseIFormFileAndMediaPathValidator());

		}
	}
}
