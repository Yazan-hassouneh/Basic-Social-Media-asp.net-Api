using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation
{
	public class AddToRoleDtoValidator : AbstractValidator<AddToRoleDto>
	{
		public AddToRoleDtoValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseRoleNameDtoValidator());
			Include(new BaseUserIdDtoValidation(userManager));
		}
	}
}
