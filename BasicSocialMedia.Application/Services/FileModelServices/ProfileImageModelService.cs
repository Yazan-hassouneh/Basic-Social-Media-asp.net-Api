using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.FileModels;
using FluentValidation;

namespace BasicSocialMedia.Application.Services.FileModelServices
{
	public class ProfileImageModelService(IUnitOfWork unitOfWork, IImageService imageService, IValidator<AddProfileImageDto> addProfileImageValidator) : IProfileImageModelService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IImageService _imageService = imageService;
		private readonly IValidator<AddProfileImageDto> _addProfileImageValidator = addProfileImageValidator;

		public Task<string?> GetUserId(int profileImageId)
		{
			return _unitOfWork.ProfileImages.GetUserId(profileImageId);
		}
		public async Task<IEnumerable<string>> GetAllImagesByUserIdAsync(string userId)
		{
			IEnumerable<ProfileImageModel?> files = await _unitOfWork.ProfileImages.GetAllAsync(model => model.UserId == userId);
			if (files == null) return Enumerable.Empty<string>();
			return files.Where(file => file != null).Select(file => file!.Path).ToList();
		}
		public async Task<bool> AddProfileImageAsync(AddProfileImageDto addProfileImageDto)
		{
			var result = await _addProfileImageValidator.ValidateAsync(addProfileImageDto);
			if (!result.IsValid) return false;

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
				// delete from projectFile
				_imageService.DeleteImage(imagePath, FileSettings.ProfileImagesPath);
				return false;
			}
		}
		public async Task<bool> UpdateMessageFileAsync(AddProfileImageDto addProfileImageDto)
		{
			var result = await _addProfileImageValidator.ValidateAsync(addProfileImageDto);
			if (!result.IsValid) return false;

			if (addProfileImageDto.CurrentImageId.HasValue)
			{
				ProfileImageModel? oldProfileImage = await _unitOfWork.ProfileImages.GetByIdWithTrackingAsync(addProfileImageDto.CurrentImageId.Value);

				if (oldProfileImage != null)
				{
					oldProfileImage.Current = false;
					_unitOfWork.ProfileImages.Update(oldProfileImage);
					int effectedRows = await _unitOfWork.ProfileImages.Save();
					if (effectedRows == 0) return false;
				}
			}

			return await AddProfileImageAsync(addProfileImageDto);
		}
		public async Task<bool> DeleteProfileImageByImageIdAsync(int imageId)
		{
			try
			{
				var image = await _unitOfWork.ProfileImages.GetByIdWithTrackingAsync(imageId);
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
