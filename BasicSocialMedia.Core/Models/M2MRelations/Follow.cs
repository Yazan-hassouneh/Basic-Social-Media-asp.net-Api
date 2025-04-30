using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.M2MRelations
{
	public class Follow : IId, ITimestamp
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? FollowerId { get; set; }
		public virtual ApplicationUser? Follower { get; set; }
		public string? FollowingId { get; set; }
		public virtual ApplicationUser? FollowingUser { get; set; }

	}
}
