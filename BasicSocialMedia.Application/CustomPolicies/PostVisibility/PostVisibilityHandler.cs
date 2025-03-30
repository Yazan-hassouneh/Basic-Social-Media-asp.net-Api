using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using Microsoft.AspNetCore.Authorization;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.CustomPolicies.PostVisibility
{
	public class PostVisibilityHandler(IFriendshipService friendshipService) : AuthorizationHandler<PostVisibilityRequirement, GetPostDto>
	{
		private readonly IFriendshipService _friendshipService = friendshipService; 

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PostVisibilityRequirement requirement, GetPostDto post)
		{
			var currentUserId = context.User.FindFirst("userId")?.Value;

			// Allow public posts for everyone, including anonymous users
			if (post.Audience == PostAudience.Public.ToString())
			{
				context.Succeed(requirement);
				return;
			}

			if (currentUserId == null)
			{
				context.Fail();
				return;
			}
			bool isOwner = currentUserId == post.UserId.ToString();
			bool isFriend = await _friendshipService.DoesFriendshipExist(currentUserId, post.UserId) != null;
			bool isSuperAdmin = context.User.IsInRole(RolesSettings.superAdminRoleName);

			// Allow access based on visibility
			if (post.Audience == PostAudience.Public.ToString() || isOwner || isSuperAdmin || (post.Audience == PostAudience.Friends.ToString() && isFriend))
			{
				context.Succeed(requirement);
			}else
			{
				context.Fail();
			}
		}
	}
}
