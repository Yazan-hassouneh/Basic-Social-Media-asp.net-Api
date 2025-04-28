using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.FilConfig
{
	internal class ProfileImagesModelConfig : IEntityTypeConfiguration<ProfileImageModel>
	{
		public void Configure(EntityTypeBuilder<ProfileImageModel> builder)
		{
			BaseFileModelConfig.ConfigureFile(builder);

			builder.Property(model => model.Current).HasDefaultValue(true);

			builder.HasOne(p => p.User) // ProfileImageModel has one User
				  .WithOne(u => u.ProfileImageModel) // User has one ProfileImageModel
				  .HasForeignKey<ProfileImageModel>(p => p.UserId) // ProfileImageModel's UserId is the foreign key
				  .OnDelete(DeleteBehavior.Cascade); // When User is deleted, delete ProfileImageModel
		}
	}
}
