namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class GetCommentFileDto
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int CommentId { get; set; }
		public int PostId { get; set; }
		public string Path { get; set; } = null!;
	}
}
