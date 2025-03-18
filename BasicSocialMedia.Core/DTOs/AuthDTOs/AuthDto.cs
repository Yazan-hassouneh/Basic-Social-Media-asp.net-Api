using System.Text.Json.Serialization;

namespace BasicSocialMedia.Core.DTOs.AuthDTOs
{
	public class AuthDto
	{
		public string Message { get; set; } = string.Empty;
		public bool IsAuthenticated { get; set; } = false;
		public string UserName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public List<string> UserRoles { get; set; } = [];
		public string Token { get; set; } = null!;
		//public DateTime ExpiresOn { get; set; }

		[JsonIgnore]
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }
	}
}
