using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class AddMessageDto : ICreateFile, ISenderIdReceiverIdDto
	{
		public List<IFormFile> Files { get; set; } = [];
		public string? Content { get; set; }
		public int ChatId { get; set; }
		public string ReceiverId { get; set; } = null!;
		[JsonIgnore] // Optional: prevents incoming data from setting it
		public string? SenderId { get; set; }
	}
}
