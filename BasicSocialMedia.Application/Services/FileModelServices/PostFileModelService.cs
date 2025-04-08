using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Application.Services.FileModelServices
{
	public class PostFileModelService(IUnitOfWork unitOfWork, IImageService imageService) : IPostFileModelService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IImageService _imageService = imageService;

		public async Task<IEnumerable<string>> GetAllFilesByPostIdAsync(int postId)
		{
			IEnumerable<PostFileModel?> files = await _unitOfWork.PostFiles.GetAllAsync(postFile => postFile.PostId == postId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<IEnumerable<string>> GetAllFilesByUserIdAsync(string userId)
		{
			IEnumerable<PostFileModel?> files = await _unitOfWork.PostFiles.GetAllAsync(postFile => postFile.UserId == userId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<bool> AddPostFileAsync(AddPostFileDto addPostFilesDto)
		{
			if (addPostFilesDto == null || addPostFilesDto.Files.Count == 0) return false;
			List<string> paths = await _imageService.GetPaths(addPostFilesDto.Files, FileSettings.PostsImagesPath, FileSettings.PostsVideosPath);
			try
			{
				// Save Paths in Database
				IEnumerable<PostFileModel> postFileModels = paths.Select(path => new PostFileModel
				{
					UserId = addPostFilesDto.UserId,
					PostId = addPostFilesDto.PostId,
					Path = path,
				}).ToList();

				await _unitOfWork.PostFiles.AddRangeAsync(postFileModels);
				await _unitOfWork.PostFiles.Save();
				return true;
			}
			catch (Exception)
			{
				foreach (var path in paths)
				{
					// delete from projectFile
					_imageService.DeleteImage(path, FileSettings.PostsImagesPath);
				}
				return false;
			}
		}
		public async Task<bool> UpdatePostFileAsync(UpdatePostFileDto updatePostFileDto)
		{
			if (updatePostFileDto.MediaPaths is null && updatePostFileDto.Files is null) return false;
			IEnumerable<PostFileModel?> files = await _unitOfWork.PostFiles.GetAllAsync(postFile => postFile.PostId == updatePostFileDto.PostId);
			if (files != null && files.Any() && updatePostFileDto.MediaPaths != null)
			{
				foreach (var file in files)
				{
					if(file != null && !updatePostFileDto.MediaPaths.Contains(file!.Path))
					{
						// delete from projectFile
						_imageService.DeleteImage(file!.Path, FileSettings.PostsImagesPath);
						// delete from database
						_unitOfWork.PostFiles.Delete(file);
						await _unitOfWork.PostFiles.Save();
					}
				}
			}

			AddPostFileDto addPostFileDto = new()
			{
				UserId = updatePostFileDto.UserId,
				PostId = updatePostFileDto.PostId,
				Files = updatePostFileDto.Files,
			};

			return await AddPostFileAsync(addPostFileDto);
		}
		public async Task<bool> DeletePostFileByFileIdAsync(int fileId)
		{
			try
			{
				var postFile = await _unitOfWork.PostFiles.GetByIdAsync(fileId);
				if (postFile == null) return false;

				_unitOfWork.PostFiles.Delete(postFile);
				int effectedRows = await _unitOfWork.PostFiles.Save();
				if (effectedRows == 0) return false;
				// delete from projectFile
				_imageService.DeleteImage(postFile.Path, FileSettings.PostsImagesPath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeletePostFileByPostIdAsync(int postId)
		{
			try
			{
				IEnumerable<PostFileModel?> postFiles = await _unitOfWork.PostFiles.GetAllAsync(postFile => postFile.PostId == postId);
				IEnumerable<PostFileModel> NonNullPostFiles = postFiles.Where(file => file != null).Select(file => file!).ToList();

				foreach (var file in NonNullPostFiles)
				{
					// delete from projectFile
					_imageService.DeleteImage(file.Path, FileSettings.PostsImagesPath);
				}

				// delete from database
				_unitOfWork.PostFiles.DeleteRange(NonNullPostFiles);
				await _unitOfWork.PostFiles.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

	}
}
