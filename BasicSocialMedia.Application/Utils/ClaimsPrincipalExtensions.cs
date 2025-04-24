using System.Security.Claims;

namespace BasicSocialMedia.Application.Utils
{
	public static class ClaimsPrincipalExtensions
	{
		public static string? GetUserId(this ClaimsPrincipal user) => user?.FindFirst("userId")?.Value;
	}
}
