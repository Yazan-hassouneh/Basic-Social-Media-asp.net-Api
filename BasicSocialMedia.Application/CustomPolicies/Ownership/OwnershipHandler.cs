using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BasicSocialMedia.Application.CustomPolicies.Ownership
{
	public class OwnershipHandler(ILogger<OwnershipHandler> logger) : AuthorizationHandler<OwnershipRequirement, string>
	{
		private readonly ILogger<OwnershipHandler> _logger = logger;

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipRequirement requirement, string userId)
		{
			if (context.User.FindFirst("userId")?.Value == userId.ToString())
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
