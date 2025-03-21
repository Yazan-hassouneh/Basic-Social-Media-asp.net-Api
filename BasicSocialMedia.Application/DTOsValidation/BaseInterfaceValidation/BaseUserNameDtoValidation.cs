using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseUserNameDtoValidation : AbstractValidator<IUserNameDto>
	{
		public BaseUserNameDtoValidation()
		{
			RuleFor(x => x.UserName)
				  .NotEmpty().WithMessage("Username is required.")
				  .Length(ModelsSettings.MinUserNameLength, ModelsSettings.MaxUserNameLength).WithMessage(ValidationSettings.UserNameErrorMessage)
				  .Matches("^[a-zA-Z0-9]*$").WithMessage("Only letters and numbers are allowed.");
		}
	}
}
