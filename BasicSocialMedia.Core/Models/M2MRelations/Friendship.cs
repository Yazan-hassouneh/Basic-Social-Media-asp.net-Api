﻿using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Core.Models.M2MRelations
{
	public class Friendship : IId, ITimestamp
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string UserId1 { get; set; } = null!;
		public ApplicationUser? User1 { get; set; }
		public string UserId2 { get; set; } = null!;
		public ApplicationUser? User2 { get; set; }
		public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
	}
}
