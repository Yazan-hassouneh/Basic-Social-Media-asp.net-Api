using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Core.Models.Notification;
using Hangfire;

namespace BasicSocialMedia.Application.BackgroundJobs
{
	internal class NotificationBackgroundJobs(IPostService postService, ICommentService commentService, IBaseNotificationService<FriendRequestNotification> friendRequestNotificationService, IBaseNotificationService<NewFollowerNotification> newFollowerNotificationService, IBaseNotificationService<PostReactionNotification> postReactionNotificationService, IBaseNotificationService<CommentReactionNotification> commentReactionNotificationService, IBaseNotificationService<NewCommentNotification> newCommentNotificationService)
	{

		private readonly IPostService _postService = postService;
		private readonly ICommentService _commentService = commentService;
		private readonly IBaseNotificationService<NewCommentNotification> _newCommentNotificationService = newCommentNotificationService;
		private readonly IBaseNotificationService<CommentReactionNotification> _commentReactionNotificationService = commentReactionNotificationService;
		private readonly IBaseNotificationService<PostReactionNotification> _postReactionNotificationService = postReactionNotificationService;
		private readonly IBaseNotificationService<NewFollowerNotification> _newFollowerNotificationService = newFollowerNotificationService;
		private readonly IBaseNotificationService<FriendRequestNotification> _friendRequestNotificationService = friendRequestNotificationService;

		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendNewCommentNotification(Comment comment)
		{
			NewCommentNotification notification = new()
			{
				NotifiedUserId = await _postService.GetUserId(comment.PostId),
				NotificationType = ProjectEnums.NotificationTypes.NewComment.ToString(),
				UserId = comment.UserId,
				PostId = comment.PostId,
				CommentId = comment.Id,
			};
			return await _newCommentNotificationService.AddNotificationAsync(notification);
		}		
		
		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendCommentReactionNotification(CommentReaction commentReaction)
		{
			GetCommentDto comment = await _commentService.GetCommentByIdAsync(commentReaction.CommentId);
			GetPostDto post = await _postService.GetPostByIdAsync(comment.PostId);

			CommentReactionNotification notification = new()
			{
				NotifiedUserId = comment.User.Id,
				NotificationType = ProjectEnums.NotificationTypes.CommentReaction.ToString(),
				UserId = commentReaction.UserId,
				PostId = post!.Id,
				CommentId = commentReaction.CommentId,
			};
			return await _commentReactionNotificationService.AddNotificationAsync(notification);
		}		
		
		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendNewFollowerNotification(Follow follow)
		{
			NewFollowerNotification notification = new()
			{
				NotifiedUserId = follow.FollowingId,
				NotificationType = ProjectEnums.NotificationTypes.NewFollower.ToString(),
				UserId = follow.FollowerId,
			};
			return await _newFollowerNotificationService.AddNotificationAsync(notification);
		}		
		
		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendPostReactionNotification(PostReaction reaction)
		{
			PostReactionNotification notification = new()
			{
				NotifiedUserId = await _postService.GetUserId(reaction.PostId),
				NotificationType = ProjectEnums.NotificationTypes.PostReaction.ToString(),
				UserId = reaction.UserId,
				PostId = reaction.PostId,
			};
			return await _postReactionNotificationService.AddNotificationAsync(notification);
		}		
		
		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendSentFriendRequestNotification(Friendship friendship)
		{
			FriendRequestNotification notification = new()
			{
				NotifiedUserId = friendship.ReceiverId,
				NotificationType = ProjectEnums.NotificationTypes.FriendRequest.ToString(),
				UserId = friendship.SenderId,
			};
			return await _friendRequestNotificationService.AddNotificationAsync(notification);
		}

		[AutomaticRetry(Attempts = 3)]
		public async Task<bool> SendAcceptedFriendRequestNotification(Friendship friendship)
		{
			FriendRequestNotification notification = new()
			{
				NotifiedUserId = friendship.SenderId,
				NotificationType = ProjectEnums.NotificationTypes.FriendRequestAccepted.ToString(),
				UserId = friendship.ReceiverId,
			};
			return await _friendRequestNotificationService.AddNotificationAsync(notification);
		}
	}
}
