
namespace BasicSocialMedia.Core.Enums
{
	public class ProjectEnums
	{
		public enum PostAudience
		{
			Public = 1,
			Friends = 2
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
			Moderator,
			User,
		}		
		public enum BackgroundJobTypes
		{
			DeleteUserAccount = 1,
		}		
		public enum NotificationTypes
		{
			NewComment = 1,
			PostReaction = 2,
			FriendRequest = 3,
			FriendRequestAccepted = 4,
			CommentReaction = 5,
			NewFollower = 6,
		}
	}
}
