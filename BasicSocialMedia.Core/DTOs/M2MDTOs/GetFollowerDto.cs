using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class GetFollowerDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string FollowerId { get; set; } = null!;
		public virtual GetBasicUserInfo? Follower { get; set; }
	}
}
