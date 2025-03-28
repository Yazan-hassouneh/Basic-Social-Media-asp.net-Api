
namespace BasicSocialMedia.Core.Enums
{
	public class ProjectEnums
	{
		public enum PostAudience
		{
			Public = 0,
			Friends = 1
		} 		
		public enum FriendshipStatus
		{
			Pending = 0,
			Accepted = 1
		}
		public enum AllowedRoles
		{
			SuperAdmin,
			Admin,
			User,
			Moderator
		}		
		public enum NotAllowedRolesToDelete
		{
			SuperAdmin,
			Admin,
			User,
		}
	}
}
