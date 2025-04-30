using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Core.Models.M2MRelations
{
	public class Friendship : IId, ITimestamp
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? SenderId { get; set; }
		public ApplicationUser? Sender { get; set; }
		public string? ReceiverId { get; set; }
		public ApplicationUser? Receiver { get; set; }
		public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
	}
}
