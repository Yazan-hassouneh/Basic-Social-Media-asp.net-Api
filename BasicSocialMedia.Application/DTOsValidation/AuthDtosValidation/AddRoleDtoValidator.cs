using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation
{
	public class AddRoleDtoValidator : AbstractValidator<AddRoleDto>
	{
		public AddRoleDtoValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseUserIdDtoValidation(userManager));

			RuleFor(x => x.RoleName)
				.NotEmpty().WithMessage("UserName is required!")
				.MaximumLength(ModelsSettings.MaxRoleNameLength).WithMessage(ValidationSettings.GeneralErrorMessage);

		}
	}
}
