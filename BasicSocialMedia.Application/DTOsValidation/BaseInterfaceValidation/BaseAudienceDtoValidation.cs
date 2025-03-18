using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseAudienceDtoValidation : AbstractValidator<IAudienceDto>
	{
		public BaseAudienceDtoValidation()
		{
			RuleFor(x => x.Audience)
				.Must(value => Enum.IsDefined(typeof(ProjectEnums.PostAudience), value))
				.WithMessage(ValidationSettings.GeneralErrorMessage);
		}
	}
}
