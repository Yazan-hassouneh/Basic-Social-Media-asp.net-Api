namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class GetMessageFileDto
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int MessageId { get; set; }
		public int ChatId { get; set; }
		public string Path { get; set; } = null!;
	}
}
