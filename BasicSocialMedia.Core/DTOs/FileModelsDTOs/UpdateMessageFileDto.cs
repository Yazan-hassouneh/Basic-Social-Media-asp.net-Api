using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class UpdateMessageFileDto : IIFormFileAndMediaPath, IMessageIdDto, IUserIdDto, IChatIdDto
	{
		public string UserId { get; set; } = null!;
		public int MessageId { get; set; }
		public int ChatId { get; set; }
		public List<IFormFile> Files { get; set; } = null!;
		public List<string>? MediaPaths { get; set; }
	}
}
