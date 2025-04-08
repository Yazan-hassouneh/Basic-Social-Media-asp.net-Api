using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class UpdateMessageDto : IUpdateFile
	{
		public int Id { get; set; }
		public string? Content { get; set; }
		public List<string>? MediaPaths { get; set; }
		public List<IFormFile> Files { get; set; } = [];
	}
}
