using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Application.Services.FileServices
{
	public class ImageService(IWebHostEnvironment env) : IImageService
	{
		private readonly IWebHostEnvironment _env = env;

		public string GetImagePath(string relativePath)
		{
			return Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
		}
		public async Task<string> SaveImage(IFormFile image, string relativePath)
		{
			string imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
			string directoryPath = GetImagePath(relativePath);

			// Ensure the directory exists
			if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

			string fullPath = Path.Combine(directoryPath, imageName);

			// Use 'using' to ensure proper disposal of the stream
			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}
			return imageName;
		}
		public void DeleteImage(string imageName, string relativePath)
		{
			var imagePath = Path.Combine(GetImagePath(relativePath), imageName);

			// Ensure file exists before deleting
			if (File.Exists(imagePath)) File.Delete(imagePath);
		}
		public async Task<string> GetMediaName(IFormFile file, string relativeImagesPath, string relativeVideosPath)
		{
			string? mediaName = string.Empty;
			if (file != null)
			{
				if (!file.ContentType.StartsWith("image/")) // Check if the file is an image
				{
					mediaName = await SaveImage(file, relativeImagesPath);
				}
				if (file.ContentType.StartsWith("video")) // Check if the file is an video
				{
					mediaName = await SaveImage(file, relativeVideosPath);
				}
			}

			return mediaName;
		}
		public async Task<List<string>> GetPaths(IEnumerable<IFormFile> files, string imageRelativePath, string videoRelativePath)
		{
			List<string> paths = [];
			foreach (var file in files)
			{
				if (file == null) continue;
				if (file.Length == 0) continue; // Check if the file is empty
												// GetMediaName will save the file and return the path
				string path = await GetMediaName(file, imageRelativePath, videoRelativePath);
				paths.Add(path);
			}
			return paths;
		}
	}
}
