using AutoMapper;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.Notification;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;
using BasicSocialMedia.Core.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class NotificationsController : ControllerBase
	{
		private readonly IBaseNotificationService<CommentReactionNotification> _commentReactionNotificationService;
		private readonly IBaseNotificationService<FriendRequestNotification> _friendRequestNotificationService;
		private readonly IBaseNotificationService<PostReactionNotification> _postReactionNotificationService;
		private readonly IBaseNotificationService<NewFollowerNotification> _newFollowerNotificationService;
		private readonly IBaseNotificationService<NewCommentNotification> _newCommentNotificationService;
		private readonly IAuthorizationService _authorizationService;

		private readonly IMapper _mapper;

		public NotificationsController(IAuthorizationService authorizationService, IMapper mapper, IBaseNotificationService<NewCommentNotification> newCommentServce, IBaseNotificationService<NewFollowerNotification> newFollowerService, IBaseNotificationService<PostReactionNotification> postReactionService, IBaseNotificationService<CommentReactionNotification> commentService,IBaseNotificationService<FriendRequestNotification> friendRequestService)
		{
			_commentReactionNotificationService = commentService;
			_friendRequestNotificationService = friendRequestService;
			_postReactionNotificationService = postReactionService;
			_newFollowerNotificationService = newFollowerService;
			_newCommentNotificationService = newCommentServce;
			_authorizationService = authorizationService;
			_mapper = mapper;
		}

		// ======== COMMENT REACTION NOTIFICATIONS ========

		[HttpGet("all")]
		public async Task<IActionResult> GetNotifications()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var commentReaction = await _commentReactionNotificationService.GetAllAsync(userId);
			var postReaction = await _postReactionNotificationService.GetAllAsync(userId);
			var friendRequest = await _friendRequestNotificationService.GetAllAsync(userId);
			var newFollower = await _newFollowerNotificationService.GetAllAsync(userId);
			var newComment = await _newCommentNotificationService.GetAllAsync(userId);

			GetNotificationDto notifications = new()
			{
				UserId = userId,
				CommentReactionNotifications = _mapper.Map<IEnumerable<CommentReactionNotificationDto>>(commentReaction),
				PostReactionNotifications = _mapper.Map<IEnumerable<PostReactionNotificationDto>>(postReaction),
				FriendRequestNotifications = _mapper.Map<IEnumerable<FriendRequestNotificationDto>>(friendRequest),
				NewFollowerNotifications = _mapper.Map<IEnumerable<NewFollowerNotificationDto>>(newFollower),
				NewCommentNotifications = _mapper.Map<IEnumerable<NewCommentNotificationDto>>(newComment)
			};
			return Ok(notifications);
		}

		[HttpPost("commentReaction/{id}/mark-as-read")]
		public async Task<IActionResult> MarkCommentNotificationAsRead(int id)
		{
			return await HandleNotificationAction(
				_commentReactionNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.MarkAsReadAsync(notificationId)
			);
		}

		[HttpDelete("commentReaction/{id}")]
		public async Task<IActionResult> DeleteCommentNotification(int id)
		{
			return await HandleNotificationAction(
				_commentReactionNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.DeleteAsync(notificationId)
			);
		}

		// ======== FRIEND REQUEST NOTIFICATIONS ========

		[HttpPost("friend-requests/{id}/mark-as-read")]
		public async Task<IActionResult> MarkFriendRequestNotificationAsRead(int id)
		{
			return await HandleNotificationAction(
				_friendRequestNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.MarkAsReadAsync(notificationId)
			);
		}

		[HttpDelete("friend-requests/{id}")]
		public async Task<IActionResult> DeleteFriendRequestNotification(int id)
		{
			return await HandleNotificationAction(
				_friendRequestNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.DeleteAsync(notificationId)
			);
		}

		// ======== Post Reaction NOTIFICATIONS ========

		[HttpPost("post-reaction/{id}/mark-as-read")]
		public async Task<IActionResult> MarkPostReactionNotificationAsRead(int id)
		{
			return await HandleNotificationAction(
				_postReactionNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.MarkAsReadAsync(notificationId)
			);
		}

		[HttpDelete("post-reaction/{id}")]
		public async Task<IActionResult> DeletePostReactionNotification(int id)
		{
			return await HandleNotificationAction(
				_postReactionNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.DeleteAsync(notificationId)
			);
		}

		// ======== New Follower NOTIFICATIONS ========

		[HttpPost("new-follower/{id}/mark-as-read")]
		public async Task<IActionResult> MarkNewFollowerNotificationAsRead(int id)
		{
			return await HandleNotificationAction(
				_newFollowerNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.MarkAsReadAsync(notificationId)
			);
		}

		[HttpDelete("new-follower/{id}")]
		public async Task<IActionResult> DeleteNewFollowerNotification(int id)
		{
			return await HandleNotificationAction(
				_newFollowerNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.DeleteAsync(notificationId)
			);
		}

		// ======== New Comment NOTIFICATIONS ========

		[HttpPost("new-comment/{id}/mark-as-read")]
		public async Task<IActionResult> MarkNewCommentNotificationAsRead(int id)
		{
			return await HandleNotificationAction(
				_newCommentNotificationService, 
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.MarkAsReadAsync(notificationId)
			);
		}

		[HttpDelete("new-comment/{id}")]
		public async Task<IActionResult> DeleteNewCommentNotification(int id)
		{
			return await HandleNotificationAction(
				_newCommentNotificationService,
				id,
				(service, notificationId) => service.GetNotifiedUserId(notificationId),
				(service, notificationId) => service.DeleteAsync(notificationId)
			);
		}

		private async Task<IActionResult> HandleNotificationAction<TService>(TService service, int id, Func<TService, int, Task<string>> getUserIdFunc, Func<TService, int, Task<bool>> actionFunc) where TService : class
		{
			var userId = await getUserIdFunc(service, id);
			var authorizationOwnership = await _authorizationService.AuthorizeAsync(User, userId, PoliciesSettings.Ownership);
			if (!authorizationOwnership.Succeeded) return Forbid();
			var result = await actionFunc(service, id);
			return result ? NoContent() : NotFound();
		}
	}
}
