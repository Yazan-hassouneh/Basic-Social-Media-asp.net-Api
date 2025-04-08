using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class AddMessageDto : ICreateFile, ISenderIdReceiverIdDto
	{
		public List<IFormFile> Files { get; set; } = [];
		public string? Content { get; set; }
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;
	}
}
