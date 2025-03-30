using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.Comment
{
	public class UpdateCommentDto : IContentDto, IUserIdDto
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string RowVersion { get; set; } = null!;// Required for concurrency handling

	}
}
