using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using Microsoft.AspNetCore.Authorization;

namespace BasicSocialMedia.Application.CustomPolicies.Block
{
	public class BlockedHandler(IBlockService blockService) : AuthorizationHandler<BlockedRequirement, string>
	{
		private readonly IBlockService _blockService = blockService;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlockedRequirement requirement, string userId)
		{
			var currentUserId = context.User.FindFirst("userId")?.Value;

			if (currentUserId == null)
			{
				context.Fail();
				return;
			}

			bool isBlocked = await _blockService.IsUsersBlocked(currentUserId, userId);

			if (isBlocked)
			{
				context.Fail();
				return ;
			}

			context.Succeed(requirement);
		}
	}
}
