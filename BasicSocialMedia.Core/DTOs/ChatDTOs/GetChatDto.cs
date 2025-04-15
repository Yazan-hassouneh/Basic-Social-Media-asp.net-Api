using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;

namespace BasicSocialMedia.Core.DTOs.ChatDTOs
{
	public class GetChatDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string User1Id { get; set; } = null!;
		public virtual GetBasicUserInfo? User1 { get; set; }
		public string User2Id { get; set; } = null!;
		public virtual GetBasicUserInfo? User2 { get; set; }
		public virtual IEnumerable<GetMessagesDto> Messages { get; set; } = [];
		public List<GetMessageFileDto>? Files { get; set; }

	}
}
