using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.ReactionsDTOs
{
	public class AddPostReactionDto : IUserIdDto, IPostIdDto
	{
		public int PostId { get; set; }
		public string UserId { get; set; } = null!;
	}
}
