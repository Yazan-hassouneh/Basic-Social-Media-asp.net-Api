using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseContentDtoValidation : AbstractValidator<IContentDto>
	{
		public BaseContentDtoValidation()
		{
			RuleFor(x => x.Content)
				.NotEmpty()
				.When(x => x.Content != null)
					.WithMessage(ValidationSettings.ContentErrorMessage);
		}
	}
}
