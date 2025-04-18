using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class UpdateCommentDto : IUserIdDto, IUpdateFile
	{
		public int Id { get; set; }
		public List<IFormFile> Files { get; set; } = [];
		public string UserId { get; set; } = null!;
		public string? Content { get; set; }
		public List<string>? MediaPaths { get; set; }
		public string RowVersion { get; set; } = null!;// Required for concurrency handling
	}
}
