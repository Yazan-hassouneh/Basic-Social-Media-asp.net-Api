using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class AddMessageDto : IContentDto
	{
		public string Content { get; set; } = null!;
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;
	}
}
