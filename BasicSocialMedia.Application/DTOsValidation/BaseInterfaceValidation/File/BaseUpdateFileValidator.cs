using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using FluentValidation;


namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File
{
	internal class BaseUpdateFileValidator : AbstractValidator<IUpdateFile>
	{
		public BaseUpdateFileValidator()
		{
			Include(new BaseIFormFile());
			Include(new BaseContentDtoValidation());

			// Ensure at least one of Content or FormFile is not null
			RuleFor(x => x)
				.Custom((post, context) =>
				{
					if (string.IsNullOrEmpty(post.Content) && post.MediaPaths == null && post.Files.Count <= 0)
					{
						context.AddFailure(ValidationSettings.GeneralErrorMessage);
					}
				});
		}
	}
}
