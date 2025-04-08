using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.FilConfig
{
	internal class CommentFileModelConfig : IEntityTypeConfiguration<CommentFileModel>
	{
		public void Configure(EntityTypeBuilder<CommentFileModel> builder)
		{
			BaseFileModelConfig.ConfigurePostComment(builder);

			builder.Property(model => model.CommentId).IsRequired();

			builder.HasOne(commentFile => commentFile.Comment)
						.WithMany(comment => comment.Files)
						.HasForeignKey(commentFile => commentFile.CommentId)
						.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(commentFile => commentFile.Post)
						.WithMany(post => post.CommentFiles)
						.HasForeignKey(commentFile => commentFile.PostId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
