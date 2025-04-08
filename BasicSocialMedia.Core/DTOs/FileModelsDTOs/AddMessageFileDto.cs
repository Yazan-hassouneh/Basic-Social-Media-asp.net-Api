using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class AddMessageFileDto : IUserIdDto, IChatIdDto, IIFormFile, IMessageIdDto
	{
		public string UserId { get; set; } = null!;
		public int MessageId { get; set; }
		public int ChatId { get; set; }
		public List<IFormFile> Files { get; set; } = null!;
	}
}
