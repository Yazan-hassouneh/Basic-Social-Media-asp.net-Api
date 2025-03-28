using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class GetFriendsDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string FriendId { get; set; } = null!;
		public GetBasicUserInfo? Friend { get; set; }
		public int Status { get; set; }
	}
}
