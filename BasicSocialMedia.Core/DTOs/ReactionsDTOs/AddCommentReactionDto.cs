using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Core.DTOs.ReactionsDTOs
{
	public class AddCommentReactionDto
	{
		public int CommentId { get; set; }
		public string UserId { get; set; } = null!;
	}
}
