using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class UpdateCommentFileDto : IMediaPaths, IIFormFile, IUserIdDto, IPostIdDto, ICommentIdDto
	{
		public string UserId { get; set; } = null!;
		public int CommentId { get; set; }
		public int PostId { get; set; }
		public List<IFormFile> Files { get; set; } = null!;
		public List<string>? MediaPaths { get; set; } = null!;

	}
}
