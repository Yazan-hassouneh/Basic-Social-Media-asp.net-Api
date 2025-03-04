using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IRoleService
	{
		Task<string> AddToRoleAsync(AddRoleDto model);
	}
}
