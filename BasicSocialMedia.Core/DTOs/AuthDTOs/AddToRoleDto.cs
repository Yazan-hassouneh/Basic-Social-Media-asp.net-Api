using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class AddToRoleDto : IUserIdDto, IRoleNameDto
	{
		public string UserId { get; set; } = null!;
		public string RoleName { get; set; } = null!;
	}
}
