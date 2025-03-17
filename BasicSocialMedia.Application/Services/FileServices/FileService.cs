using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Application.Services.FileServices
{
	public class FileService(IWebHostEnvironment env) : IFileService
	{
		private readonly IWebHostEnvironment _env = env;
		private string _imagePath = null!;
		public void SetImagePath(string path)
		{
			this._imagePath = $"{_env.WebRootPath}{path}";
		}
		public async Task<string> SaveImage(IFormFile image)
		{
			string imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
			var path = Path.Combine(_imagePath, imageName);

			using var stream = File.Create(path);
			await image.CopyToAsync(stream);
			stream.Dispose();

			return imageName;
		}
		public void DeleteImage(string imageName)
		{
			var image = Path.Combine(_imagePath, imageName);
			File.Delete(image);
		}
	}
}
