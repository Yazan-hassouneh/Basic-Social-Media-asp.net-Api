using BasicSocialMedia.Application.Services.FileServices;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using System.ComponentModel.Design;

namespace BasicSocialMedia.Application.Services.FileModelServices
{
	public class CommentFileModelService(IUnitOfWork unitOfWork, IImageService imageService) : ICommentFileModeService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IImageService _imageService = imageService;

		public async Task<bool> AddCommentFileAsync(AddCommentFileDto addCommentFilesDto)
		{
			if (addCommentFilesDto == null || addCommentFilesDto.Files.Count == 0) return false;
			List<string> paths = await _imageService.GetPaths(addCommentFilesDto.Files, FileSettings.CommentsImagesPath, FileSettings.CommentsVideosPath);
			try
			{
				// Save Paths in Database
				IEnumerable<CommentFileModel> commentFileModels = paths.Select(path => new CommentFileModel
				{
					UserId = addCommentFilesDto.UserId,
					PostId = addCommentFilesDto.PostId,
					CommentId = addCommentFilesDto.CommentId,
					Path = path,
				}).ToList();

				await _unitOfWork.CommentFiles.AddRangeAsync(commentFileModels);
				await _unitOfWork.CommentFiles.Save();
				return true;
			}
			catch (Exception)
			{
				foreach (var path in paths)
				{
					// delete from projectFile
					_imageService.DeleteImage(path, FileSettings.CommentsImagesPath);
				}
				return false;
			}
		}
		public async Task<bool> UpdateCommentFileAsync(UpdateCommentFileDto updateCommentFileDto)
		{
			if (updateCommentFileDto.MediaPaths is null && updateCommentFileDto.Files is null) return false;
			IEnumerable<CommentFileModel?> files = await _unitOfWork.CommentFiles.GetAllAsync(commentFile => commentFile.CommentId == updateCommentFileDto.CommentId);
			if (files != null && files.Any() && updateCommentFileDto.MediaPaths != null)
			{
				foreach (var file in files)
				{
					if (file != null && !updateCommentFileDto.MediaPaths.Contains(file!.Path))
					{
						// delete from projectFile
						_imageService.DeleteImage(file!.Path, FileSettings.CommentsImagesPath);
						// delete from database
						_unitOfWork.CommentFiles.Delete(file);
						await _unitOfWork.CommentFiles.Save();
					}
				}
			}

			AddCommentFileDto addCommentFileDto = new()
			{
				UserId = updateCommentFileDto.UserId,
				PostId = updateCommentFileDto.PostId,
				CommentId = updateCommentFileDto.CommentId,
				Files = updateCommentFileDto.Files,
			};

			return await AddCommentFileAsync(addCommentFileDto);
		}
		public async Task<bool> DeleteCommentFileByCommentIdAsync(int commentId)
		{
			try
			{
				IEnumerable<CommentFileModel?> commentFiles = await _unitOfWork.CommentFiles.GetAllAsync(commentFile => commentFile.CommentId == commentId);
				IEnumerable<CommentFileModel> NonNullCommentFiles = commentFiles.Where(file => file != null).Select(file => file!).ToList();

				foreach (var file in NonNullCommentFiles)
				{
					// delete from projectFile
					_imageService.DeleteImage(file.Path, FileSettings.PostsImagesPath);
				}

				// delete from database
				_unitOfWork.CommentFiles.DeleteRange(NonNullCommentFiles);
				await _unitOfWork.CommentFiles.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteCommentFileByFileIdAsync(int fileId)
		{
			try
			{
				var commentFile = await _unitOfWork.CommentFiles.GetByIdAsync(fileId);
				if (commentFile == null) return false;

				_unitOfWork.CommentFiles.Delete(commentFile);
				int effectedRows = await _unitOfWork.CommentFiles.Save();
				if (effectedRows == 0) return false;
				// delete from projectFile
				_imageService.DeleteImage(commentFile.Path, FileSettings.CommentsImagesPath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteCommentFileByPostIdAsync(int postId)
		{
			try
			{
				IEnumerable<CommentFileModel?> commentFiles = await _unitOfWork.CommentFiles.GetAllAsync(commentFile => commentFile.PostId == postId);
				IEnumerable<CommentFileModel> NonNullCommentFiles = commentFiles.Where(file => file != null).Select(file => file!).ToList();

				foreach (var file in NonNullCommentFiles)
				{
					// delete from projectFile
					_imageService.DeleteImage(file.Path, FileSettings.PostsImagesPath);
				}

				// delete from database
				_unitOfWork.CommentFiles.DeleteRange(NonNullCommentFiles);
				await _unitOfWork.CommentFiles.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
