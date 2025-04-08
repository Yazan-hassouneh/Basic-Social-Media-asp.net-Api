using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class AddCommentDto : IUserIdDto, IPostIdDto, ICreateFile
	{
		public List<IFormFile> Files { get; set; } = [];
		public string? Content { get; set; }
		public string UserId { get; set; } = null!;
		public int PostId { get; set; }
	}
}
