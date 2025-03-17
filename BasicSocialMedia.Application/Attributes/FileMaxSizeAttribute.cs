using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Application.Attributes
{
	public class FileMaxSizeAttribute(int maxSize) : ValidationAttribute
	{
		private readonly int _maxSize = maxSize;

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file is not null)
			{
				if (file.Length > _maxSize)
				{
					return new ValidationResult("File Is Too Large");
				}
			}
			return ValidationResult.Success;
		}
	}
}
