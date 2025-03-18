using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BasePasswordDtoValidation : AbstractValidator<IPasswordDto>
	{
		public BasePasswordDtoValidation()
		{
			RuleFor(x => x.Password)
				.NotNull().NotEmpty().WithMessage(ValidationSettings.ContentErrorMessage)
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
		}
	}
}
