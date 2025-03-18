using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseEmailDtoValidation : AbstractValidator<IEmailDto>
	{
		public BaseEmailDtoValidation()
		{
			RuleFor(x => x.Email)
				.NotNull().NotEmpty().WithMessage(ValidationSettings.GeneralErrorMessage)
				.EmailAddress().WithMessage(ValidationSettings.EmailErrorMessage);
		}
	}
}
