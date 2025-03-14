using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class GetMessagesDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string Content { get; set; } = null!;
		public bool IsRead { get; set; } = false;
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;
	}
}
