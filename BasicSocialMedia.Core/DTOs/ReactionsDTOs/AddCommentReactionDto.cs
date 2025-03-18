using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;

namespace BasicSocialMedia.Core.DTOs.ReactionsDTOs
{
	public class AddCommentReactionDto : IUserIdDto, ICommentIdDto
	{
		public int CommentId { get; set; }
		public string UserId { get; set; } = null!;
	}
}
