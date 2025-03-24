using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IRoleService
	{
		Task<string> AddToRoleAsync(AddToRoleDto model);
		Task<string> AddNewRoleAsync(AddRoleDto roleDto);
		Task<string?> DeleteRoleAsync(AddRoleDto roleDto);
	}
}
