using BasicSocialMedia.Core.DTOs.ProfileImage;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices
{
	public interface IProfileImageModelService
	{
		Task<IEnumerable<string>> GetAllImagesByUserIdAsync(string userId);
		Task<bool> AddProfileImageAsync(AddProfileImageDto addProfileImageDto);
		Task<bool> UpdateMessageFileAsync(AddProfileImageDto addProfileImageDto);
		Task<bool> DeleteProfileImageByImageIdAsync(int imageId);
		Task<bool> DeleteProfileImageByUserIdAsync(string userId);
		Task<string?> GetUserId(int profileImage);
	}
}
