using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Tables_Schema
{
	internal static class IdentityTables
	{
		public static void AppleyIdentityTablesConfig(this ModelBuilder builder)
		{
			builder.Entity<IdentityRole>().ToTable("Roles", schema: "Auth");
			builder.Entity<ApplicationUser>().ToTable("Users", schema: "Auth");
			builder.Entity<UserBackgroundJob>().ToTable("UserBackgroundJobs", schema: "Auth");
			//builder.Entity<RefreshToken>().ToTable("RefreshTokens", schema: "Auth");
			builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema: "Auth");
			builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schema: "Auth");
			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schema: "Auth");
			builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema: "Auth");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schema: "Auth");

		}
	}
}
