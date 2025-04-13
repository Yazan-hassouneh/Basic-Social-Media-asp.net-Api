using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Infrastructure.Configuration.FilConfig
{
	internal class PostFileModelConfig : IEntityTypeConfiguration<PostFileModel>
	{
		public void Configure(EntityTypeBuilder<PostFileModel> builder)
		{
			BaseFileModelConfig.ConfigureFile(builder);

			builder.Property(model => model.PostId).IsRequired();

			builder.HasOne(postFile => postFile.Post)
						.WithMany(post => post.Files)
						.HasForeignKey(postFile => postFile.PostId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
