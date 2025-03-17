using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices
{
	public interface IFileService
	{
		public void SetImagePath(string path);
		public Task<string> SaveImage(IFormFile image);
		public void DeleteImage(string imageName);
	}
}
