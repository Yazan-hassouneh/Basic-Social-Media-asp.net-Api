using BasicSocialMedia.Application.Helpers;
using Microsoft.Extensions.Options;

namespace BasicSocialMedia.Web.Middlewares
{
	public class SecretKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<SecretKeyMiddleware> logger, IOptions<SecuritySettings> securitySettings	)
	{
		private readonly RequestDelegate _next = next;
		private readonly ILogger<SecretKeyMiddleware> _logger = logger;
		private readonly SecuritySettings _securitySettings = securitySettings.Value;

		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue("Secret-Key", out var providedKey) || providedKey != _securitySettings.FrontendSecretKey)
			{
				_logger.LogWarning("Unauthorized request due to missing or invalid secret key.");
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized: Invalid secret key.");
				return;
			}

			await _next(context); // Continue to the next middleware or controller
		}

		/*
			fetch("https://yourapi.com/getAllByUserFriends", {
				headers: {
					"Authorization": "Bearer " + userToken,
					"Secret-Key": "YOUR_SECURE_KEY"
				}
			});
		 */
	}
}
