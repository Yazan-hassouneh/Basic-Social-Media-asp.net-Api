using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class GetFollowingDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string FollowingId { get; set; } = null!;
		public virtual GetBasicUserInfo? FollowingUser { get; set; }
	}
}
