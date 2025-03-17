using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Application.Attributes
{
	public class AllowedExtensionsAttribute(string allowedExtensions) : ValidationAttribute
	{
		private readonly string _allowedExtensions = allowedExtensions;

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file is not null)
			{
				var extension = Path.GetExtension(file.FileName);
				var isAllowed = _allowedExtensions.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);

				if (!isAllowed)
				{
					return new ValidationResult($"Only {_allowedExtensions} Is Allowed");
				}
			}
			return ValidationResult.Success;
		}
	}
}
