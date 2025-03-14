using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.ReactionsDTOs
{
	public class GetReactionDto
	{
		public int Id { get; set; }
		public virtual GetBasicUserInfo? User { get; set; }
	}
}
