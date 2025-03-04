using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces
{
	public interface IRoleService
	{
		Task<string> AddToRoleAsync(AddRoleDto model);
	}
}
