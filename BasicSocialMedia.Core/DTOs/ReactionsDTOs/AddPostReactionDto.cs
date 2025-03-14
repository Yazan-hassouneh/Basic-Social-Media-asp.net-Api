using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Core.DTOs.ReactionsDTOs
{
	public class AddPostReactionDto
	{
		public int PostId { get; set; }
		public string UserId { get; set; } = null!;
	}
}
