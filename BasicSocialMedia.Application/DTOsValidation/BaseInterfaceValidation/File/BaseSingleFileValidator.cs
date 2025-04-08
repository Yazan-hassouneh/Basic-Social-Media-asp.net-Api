using BasicSocialMedia.Core.Consts;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File
{
	public class BaseSingleFileValidator : AbstractValidator<IFormFile>
	{
		public BaseSingleFileValidator()
		{
			RuleFor(x => x)
				.NotNull().WithMessage("File is required.")
				.Must(file => file.Length > 0).WithMessage("File cannot be empty.");

			RuleFor(x => x)
				.Must(file => IsValidImage(file) || IsValidVideo(file))
				.WithMessage("Invalid file type or size.");
		}
		private static bool IsValidImage(IFormFile file)
		{
			if (!file.ContentType.StartsWith("image/")) return false;

			var allowedExtensions = FileSettings.ImagesAllowedExtension.Split(',');
			var extension = Path.GetExtension(file.FileName)?.ToLower();

			return file.Length <= FileSettings.ImagesMaxSizeInBytes && allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
		}

		private static bool IsValidVideo(IFormFile file)
		{
			if (!file.ContentType.StartsWith("video/"))
				return false;

			var allowedExtensions = FileSettings.VideoAllowedExtension.Split(',');
			var extension = Path.GetExtension(file.FileName)?.ToLower();

			return file.Length <= FileSettings.VideoMaxSizeInBytes &&
				   allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
		}
	}
}
