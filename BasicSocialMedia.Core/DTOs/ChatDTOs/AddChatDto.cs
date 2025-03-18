using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.ChatDTOs
{
	public class AddChatDto : ISenderIdReceiverIdDto
	{
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;
	}
}
