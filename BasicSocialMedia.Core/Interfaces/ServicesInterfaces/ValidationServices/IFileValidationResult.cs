using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.ValidationServices
{
	public interface IFileValidationResult
	{
		public Task<IEnumerable<IFormFile>> ValidateFiles(IEnumerable<IFormFile> files);
	}
}
