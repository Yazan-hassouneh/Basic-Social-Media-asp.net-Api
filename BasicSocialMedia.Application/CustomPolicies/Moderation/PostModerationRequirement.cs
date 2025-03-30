using Microsoft.AspNetCore.Authorization;

namespace BasicSocialMedia.Application.CustomPolicies.Moderation
{
	public class PostModerationRequirement(string permission) : IAuthorizationRequirement
	{
		public string Permission { get; } = permission;
	}
}
