using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Application.Services.FileModelServices
{
	public class MessageFileModelService(IUnitOfWork unitOfWork, IImageService imageService) : IMessageFileModelService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IImageService _imageService = imageService;

		public async Task<IEnumerable<string>> GetAllFilesByChatIdAsync(int chatId)
		{
			IEnumerable<MessageFileModel?> files = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.ChatId == chatId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<IEnumerable<string>> GetAllFilesByMessageIdAsync(int messageId)
		{
			IEnumerable<MessageFileModel?> files = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.MessageId == messageId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<IEnumerable<string>> GetAllFilesByUserIdAsync(string userId)
		{
			IEnumerable<MessageFileModel?> files = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.UserId == userId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<bool> AddMessageFileAsync(AddMessageFileDto addMessageFilesDto)
		{
			if (addMessageFilesDto == null || addMessageFilesDto.Files.Count == 0) return false;
			List<string> paths = await _imageService.GetPaths(addMessageFilesDto.Files, FileSettings.MessagesImagesPath, FileSettings.MessagesVideosPath);
			try
			{
				// Save Paths in Database
				IEnumerable<MessageFileModel> messageFileModels = paths.Select(path => new MessageFileModel
				{
					UserId = addMessageFilesDto.UserId,
					MessageId = addMessageFilesDto.MessageId,
					ChatId = addMessageFilesDto.ChatId,
					Path = path,
				}).ToList();

				await _unitOfWork.MessageFiles.AddRangeAsync(messageFileModels);
				await _unitOfWork.MessageFiles.Save();
				return true;
			}
			catch (Exception)
			{
				foreach (var path in paths)
				{
					// delete from projectFile
					_imageService.DeleteImage(path, FileSettings.MessagesImagesPath);
				}
				return false;
			}
		}
		public async Task<bool> UpdateMessageFileAsync(UpdateMessageFileDto updateMessageFileDto)
		{
			if (updateMessageFileDto.MediaPaths is null && updateMessageFileDto.Files is null) return false;
			IEnumerable<MessageFileModel?> files = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.MessageId == updateMessageFileDto.MessageId);
			if (files != null && files.Any() && updateMessageFileDto.MediaPaths != null)
			{
				foreach (var file in files)
				{
					if (file != null && !updateMessageFileDto.MediaPaths.Contains(file!.Path))
					{
						// delete from projectFile
						_imageService.DeleteImage(file!.Path, FileSettings.MessagesImagesPath);
						// delete from database
						_unitOfWork.MessageFiles.Delete(file);
						await _unitOfWork.MessageFiles.Save();
					}
				}
			}

			AddMessageFileDto addMessageFileDto = new()
			{
				UserId = updateMessageFileDto.UserId,
				MessageId = updateMessageFileDto.MessageId,
				ChatId = updateMessageFileDto.ChatId,
				Files = updateMessageFileDto.Files
			};

			return await AddMessageFileAsync(addMessageFileDto);
		}
		public async Task<bool> DeleteMessageFileByChatIdAsync(int chatId)
		{
			try
			{
				IEnumerable<MessageFileModel?> messageFiles = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.ChatId == chatId);
				IEnumerable<MessageFileModel> NonNullMessageFile = messageFiles.Where(file => file != null).Select(file => file!).ToList();

				foreach (var file in NonNullMessageFile)
				{
					// delete from projectFile
					_imageService.DeleteImage(file.Path, FileSettings.MessagesImagesPath);
				}

				// delete from database
				_unitOfWork.MessageFiles.DeleteRange(NonNullMessageFile);
				await _unitOfWork.MessageFiles.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteMessageFileByFileIdAsync(int fileId)
		{
			try
			{
				var messageFile = await _unitOfWork.MessageFiles.GetByIdAsync(fileId);
				if (messageFile == null) return false;

				_unitOfWork.MessageFiles.Delete(messageFile);
				int effectedRows = await _unitOfWork.MessageFiles.Save();
				if (effectedRows == 0) return false;
				// delete from projectFile
				_imageService.DeleteImage(messageFile.Path, FileSettings.MessagesImagesPath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteMessageFileByMessageIdAsync(int messageId)
		{
			try
			{
				IEnumerable<MessageFileModel?> messageFiles = await _unitOfWork.MessageFiles.GetAllAsync(messageFile => messageFile.MessageId == messageId);
				IEnumerable<MessageFileModel> NonNullMessageFile = messageFiles.Where(file => file != null).Select(file => file!).ToList();

				foreach (var file in NonNullMessageFile)
				{
					// delete from projectFile
					_imageService.DeleteImage(file.Path, FileSettings.MessagesImagesPath);
				}

				// delete from database
				_unitOfWork.MessageFiles.DeleteRange(NonNullMessageFile);
				await _unitOfWork.MessageFiles.Save();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
