using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class UpdateMessageDto : IContentDto
	{
		public int Id { get; set; }
		public string Content { get; set; } = null!;
	}
}
