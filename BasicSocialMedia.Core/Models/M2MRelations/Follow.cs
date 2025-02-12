using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.M2MRelations
{
	public class Follow : IId, ITimestamp
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public bool IsBlocked { get; set; } = false;
		public string FollowerId { get; set; } = null!;
		public virtual ApplicationUser? Follower { get; set; }
		public string FollowingId { get; set; } = null!;
		public virtual ApplicationUser? FollowingUser { get; set; }

	}
}
