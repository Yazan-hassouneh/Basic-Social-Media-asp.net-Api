using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.ValidationServices;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Application.Services.ValidationServices
{
	public class FileValidationResult(IValidator<IFormFile> formFileValidator) : IFileValidationResult
	{

		private readonly IValidator<IFormFile> _formFileValidator = formFileValidator;

		public async Task<IEnumerable<IFormFile>> ValidateFiles(IEnumerable<IFormFile> files)
		{
			if (files == null || !files.Any()) return [];

			List<IFormFile> validFiles = new();
			foreach (var file in files)
			{
				var validationResult = await _formFileValidator.ValidateAsync(file);
				if (validationResult.IsValid) validFiles.Add(file);
			}

			return validFiles;
		}
	}
}
