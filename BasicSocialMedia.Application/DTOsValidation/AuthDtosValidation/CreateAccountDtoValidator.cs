using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation
{
	public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
	{
		public CreateAccountDtoValidator()
		{
			Include(new BaseEmailDtoValidation());
			Include(new BasePasswordDtoValidation());

			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("UserName is required!")
				.MaximumLength(ModelsSettings.MaxUserNameLength).WithMessage(ValidationSettings.GeneralErrorMessage);

		}
	}
}
