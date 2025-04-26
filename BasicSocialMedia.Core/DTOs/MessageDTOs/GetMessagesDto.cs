

using BasicSocialMedia.Core.DTOs.FileModelsDTOs;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class GetMessagesDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? Content { get; set; }
		public List<GetMessageFileDto>? Files { get; set; }
		public bool IsRead { get; set; } = false;
		public string SenderId { get; set; } = null!;
		public string ReceiverId { get; set; } = null!;
	}
}
