using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File
{
	internal class BaseIFormFileAndMediaPathValidator : AbstractValidator<IIFormFileAndMediaPath>
	{
		public BaseIFormFileAndMediaPathValidator()
		{
			RuleFor(x => x)
				.Custom((user, context) =>
				{
					if (user.MediaPaths is null && user.Files is null)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
					if (user.MediaPaths!.Count == 0 && user.Files!.Count == 0)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
				});
		}
	}
}
