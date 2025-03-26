namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class SendFollowRequestDto
	{
		public string FollowerId { get; set; } = null!;
		public string FollowingId { get; set; } = null!;
	}
}
