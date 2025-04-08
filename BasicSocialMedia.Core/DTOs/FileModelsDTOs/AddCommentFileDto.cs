using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class AddCommentFileDto : IUserIdDto, IPostIdDto, IIFormFile, ICommentIdDto
	{
		public string UserId { get; set; } = null!;
		public int CommentId { get; set; }
		public int PostId { get; set; }
		public List<IFormFile> Files { get; set; } = null!;
	}
}
