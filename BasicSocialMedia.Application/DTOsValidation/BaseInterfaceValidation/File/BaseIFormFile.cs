using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File
{
	internal class BaseIFormFile : AbstractValidator<IIFormFile>
	{
		public BaseIFormFile()
		{
			RuleFor(x => x)
				.Must(file => file.Files.Count >= 0);
		}
	}
}
