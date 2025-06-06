﻿using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class GetCommentDto
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public DateTime CreatedOn { get; set; }
		public int ReactionsCount { get; set; }
		public string RowVersion { get; set; } = null!;
		public string? Content { get; set; }
		public List<GetCommentFileDto>? Files { get; set; }
		public GetBasicUserInfo User { get; set; } = null!;
		public List<GetBasicUserInfo>? ReactionsList { get; set; }
	}
}
