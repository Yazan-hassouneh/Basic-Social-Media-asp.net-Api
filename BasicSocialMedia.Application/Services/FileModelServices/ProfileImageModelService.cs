using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using static System.Net.Mime.MediaTypeNames;

namespace BasicSocialMedia.Application.Services.FileModelServices
{
	public class ProfileImageModelService(IUnitOfWork unitOfWork, IImageService imageService) : IProfileImageModelService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IImageService _imageService = imageService;

		public async Task<IEnumerable<string>> GetAllImagesByUserIdAsync(string userId)
		{
			IEnumerable<ProfileImageModel?> files = await _unitOfWork.ProfileImages.GetAllAsync(model => model.UserId == userId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<bool> AddProfileImageAsync(AddProfileImageDto addProfileImageDto)
		{
			if (addProfileImageDto == null || addProfileImageDto.Image.Length == 0) return false;
			string imagePath = await _imageService.SaveImage(addProfileImageDto.Image, FileSettings.ProfileImagesPath);
			try
			{
				// Save Paths in Database
				ProfileImageModel profileImageModel = new()
				{
					UserId = addProfileImageDto.UserId,
					Path = imagePath,
				};

				await _unitOfWork.ProfileImages.AddAsync(profileImageModel);
				await _unitOfWork.ProfileImages.Save();
				return true;
			}
			catch (Exception)
			{
				_imageService.DeleteImage(imagePath, FileSettings.ProfileImagesPath);
				return false;
			}
		}
		public async Task<bool> UpdateMessageFileAsync(UpdateProfileImageDto updateProfileImageDto)
		{
			if (updateProfileImageDto == null || updateProfileImageDto.Image!.Length == 0) return false;
			ProfileImageModel? oldProfileImage = await _unitOfWork.ProfileImages.FindWithTrackingAsync(model => model.UserId == updateProfileImageDto.UserId);

			if (oldProfileImage != null && !string.IsNullOrEmpty(updateProfileImageDto.ImagePath))
			{
				// delete from projectFile
				_imageService.DeleteImage(oldProfileImage.Path, FileSettings.ProfileImagesPath);
				// delete from database
				_unitOfWork.ProfileImages.Delete(oldProfileImage);
				await _unitOfWork.ProfileImages.Save();
			}

			AddProfileImageDto addPostFileDto = new()
			{
				UserId = updateProfileImageDto.UserId,
				Image = updateProfileImageDto.Image,
			};

			return await AddProfileImageAsync(addPostFileDto);
		}
		public async Task<bool> DeleteProfileImageByImageIdAsync(int imageId)
		{
			try
			{
				var image = await _unitOfWork.ProfileImages.GetByIdAsync(imageId);
				if (image == null) return false;

				_unitOfWork.ProfileImages.Delete(image);
				int effectedRows = await _unitOfWork.ProfileImages.Save();
				if (effectedRows == 0) return false;
				// delete from projectFile
				_imageService.DeleteImage(image.Path, FileSettings.ProfileImagesPath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteProfileImageByUserIdAsync(string userId)
		{
			try
			{
				var image = await _unitOfWork.ProfileImages.FindWithTrackingAsync(model => model.UserId == userId);
				if (image == null) return false;

				_unitOfWork.ProfileImages.Delete(image);
				int effectedRows = await _unitOfWork.ProfileImages.Save();
				if (effectedRows == 0) return false;
				// delete from projectFile
				_imageService.DeleteImage(image.Path, FileSettings.ProfileImagesPath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
