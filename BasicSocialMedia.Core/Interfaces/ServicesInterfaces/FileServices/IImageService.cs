using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices
{
	public interface IImageService
	{
		string GetImagePath(string relativePath);
		public Task<string> SaveImage(IFormFile image, string relativePath);
		public void DeleteImage(string imageName, string relativePath);
		public Task<string> GetMediaName(IFormFile file, string relativeImagesPath, string relativeVideosPath);
		public Task<List<string>> GetPaths(IEnumerable<IFormFile> files, string imageRelativePath, string videoRelativePath);
	}
}
