namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class MessageExistDto
	{
		public int Id { get; set; }
		public string SenderId { get; set; } = null!;
		public string ReceiverId { get; set; } = null!;
	}
}
