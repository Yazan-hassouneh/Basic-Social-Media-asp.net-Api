using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class PostConfig : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			BasePostCommentConfig.ConfigurePostComment(builder);

			builder.HasOne(post => post.User)
				   .WithMany(user => user.Posts)
				   .HasForeignKey(post => post.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
