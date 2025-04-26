using System.Text.Json.Serialization;

namespace BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base
{
	public interface ISenderIdReceiverIdDto
	{
		public string ReceiverId { get; set; }
		[JsonIgnore]
		public string? SenderId { get; set; }
	}
}
