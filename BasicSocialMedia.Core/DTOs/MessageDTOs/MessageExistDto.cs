namespace BasicSocialMedia.Core.DTOs.MessageDTOs
{
	public class MessageExistDto
	{
		public int Id { get; set; }
		public string User1Id { get; set; } = null!;
		public string User2Id { get; set; } = null!;
	}
}
