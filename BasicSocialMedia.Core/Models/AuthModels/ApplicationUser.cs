using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Core.Models.MainModels;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Core.Models.AuthModels
{
	public class ApplicationUser : IdentityUser
	{
		public string? Bio { get; set; }
		public string? ProfileImage { get; set; }
		public bool IsDeleted { get; set; } = false;
		public bool AllowFriendships { get; set; } = true;
		public DateTime? BirthDate { get; set; }
		public DateTime JoinedAt { get; set; }
		public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
		public virtual ICollection<Post> Posts { get; set; } = [];
		public virtual ICollection<Follow> Followers { get; set; } = [];
		public virtual ICollection<Follow> Following { get; set; } = [];
		public virtual ICollection<Friendship> Friendships { get; set; } = [];
		public virtual ICollection<Chat> Chats { get; set; } = [];
	}
}
