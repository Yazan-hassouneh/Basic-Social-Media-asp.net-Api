using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseRoleNameDtoValidator : AbstractValidator<IRoleNameDto>
	{
		public BaseRoleNameDtoValidator()
		{
			RuleFor(x => x.RoleName)
				 .NotNull().NotEmpty().WithMessage("RoleName is required!")
				.MaximumLength(ModelsSettings.MaxRoleNameLength).WithMessage(ValidationSettings.GeneralErrorMessage);
		}
	}
}
