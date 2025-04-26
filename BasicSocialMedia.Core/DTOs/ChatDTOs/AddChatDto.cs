using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using System.Text.Json.Serialization;

namespace BasicSocialMedia.Core.DTOs.ChatDTOs
{
	public class AddChatDto : ISenderIdReceiverIdDto
	{
		public string ReceiverId { get; set; } = null!;
		[JsonIgnore]
		public string? SenderId { get; set; }
	}
}
