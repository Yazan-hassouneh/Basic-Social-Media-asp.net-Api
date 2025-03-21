using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class AddRoleDto : IUserIdDto
	{
		public string UserId { get; set; } = null!;
		public string RoleName { get; set; } = null!;
	}
}
