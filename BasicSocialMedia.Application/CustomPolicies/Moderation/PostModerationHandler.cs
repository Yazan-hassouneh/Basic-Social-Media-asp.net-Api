using BasicSocialMedia.Core.Consts;
using Microsoft.AspNetCore.Authorization;

namespace BasicSocialMedia.Application.CustomPolicies.Moderation
{
	public class PostModerationHandler : AuthorizationHandler<PostModerationRequirement, string>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PostModerationRequirement requirement, string userId)
		{
			bool hasPermission = false;

			// Admins and SuperAdmins can do any moderation action (edit/delete)
			if (context.User.IsInRole(RolesSettings.superAdminRoleName))
			{
				hasPermission = true;
			}
			// Moderators can delete any post but can't edit
			else if (context.User.IsInRole(RolesSettings.ModeratorRoleName) && requirement.Permission == "Delete")
			{
				hasPermission = true;
			}
			// Regular users can only edit/delete their own posts
			else if (context.User.IsInRole(RolesSettings.userRoleName) && requirement.Permission == "Edit" && context.User.FindFirst("userId")?.Value == userId.ToString())
			{
				hasPermission = true;
			}
			else if (context.User.IsInRole(RolesSettings.userRoleName) && requirement.Permission == "Delete" && context.User.FindFirst("userId")?.Value == userId.ToString())
			{
				hasPermission = true;
			}

			if (hasPermission)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
