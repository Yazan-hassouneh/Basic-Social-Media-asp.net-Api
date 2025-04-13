
namespace BasicSocialMedia.Core.DTOs.FileModelsDTOs
{
	public class GetPostFilesDto
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int PostId { get; set; }
		public string Path { get; set; } = null!;
	}
}
