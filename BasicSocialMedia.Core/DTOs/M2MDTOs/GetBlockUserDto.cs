using BasicSocialMedia.Core.DTOs.AuthDTOs;

namespace BasicSocialMedia.Core.DTOs.M2MDTOs
{
	public class GetBlockUserDto
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string BlockerId { get; set; } = null!;
		public string BlockedId { get; set; } = null!;
		public GetBasicUserInfo? Blocked { get; set; }
	}
}
