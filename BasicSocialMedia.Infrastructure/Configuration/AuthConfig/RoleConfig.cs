using BasicSocialMedia.Core.Consts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Infrastructure.Configuration.AuthConfig
{
	internal class RoleConfig : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.Property(model => model.Name).IsRequired().HasMaxLength(ModelsSettings.MaxRoleNameLength);
		}
	}
}
