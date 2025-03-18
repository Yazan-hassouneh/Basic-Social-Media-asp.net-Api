using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation
{
	public class LoginAccountDtoValidator : AbstractValidator<LoginAccountDto>
	{
		public LoginAccountDtoValidator()
		{
			Include(new BaseEmailDtoValidation());
			Include(new BasePasswordDtoValidation());
		}
	}
}
