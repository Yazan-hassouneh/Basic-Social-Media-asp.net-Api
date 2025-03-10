using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class GetBasicUserInfo
	{
		public string Id { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string ProfileImage { get; set; } = null!;
	}
}
