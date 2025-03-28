

namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class SendFriendRequestDto
	{
		public string SenderId { get; set; } = null!;
		public string ReceiverId { get; set; } = null!;
	}
}
