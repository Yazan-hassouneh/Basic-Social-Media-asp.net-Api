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
		}
	}
}
