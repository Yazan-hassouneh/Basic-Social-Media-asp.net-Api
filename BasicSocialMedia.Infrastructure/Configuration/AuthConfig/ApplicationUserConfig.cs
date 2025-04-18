using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Infrastructure.Configuration.AuthConfig
{
	internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(model => model.UserName).IsRequired().HasMaxLength(ModelsSettings.MaxUserNameLength);
			builder.Property(model => model.NormalizedUserName).IsRequired().HasMaxLength(ModelsSettings.MaxUserNameLength);
			builder.Property(model => model.IsDeleted).IsRequired().HasDefaultValue(false);
			builder.Property(model => model.AllowFriendships).IsRequired().HasDefaultValue(true);
			builder.Property(model => model.JoinedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
			builder.Property(model => model.Bio).IsRequired(false).HasMaxLength(ModelsSettings.MaxUserBioLength);
			builder.Property(model => model.BirthDate).IsRequired(false);

			builder.HasOne(a => a.ProfileImageModel)
				.WithOne(p => p.User)
				.HasForeignKey<ProfileImageModel>(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasQueryFilter(u => !u.IsDeleted);
		}
	}
}
