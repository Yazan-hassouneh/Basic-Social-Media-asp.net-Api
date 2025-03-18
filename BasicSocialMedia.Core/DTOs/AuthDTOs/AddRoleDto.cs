using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class AddRoleDto : IUserIdDto
	{
		public string UserId { get; set; } = null!;
		public string RoleName { get; set; } = null!;
	}
}
