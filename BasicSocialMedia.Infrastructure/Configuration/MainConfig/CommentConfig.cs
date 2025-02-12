using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class CommentConfig : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			BasePostCommentConfig.ConfigurePostComment(builder);

			builder.HasOne(comment => comment.User)
					.WithMany()
					.HasForeignKey(comment => comment.UserId)
					.HasPrincipalKey(user => user.Id)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(comment => comment.Post)
						.WithMany(post => post.Comments)
						.HasForeignKey(comment => comment.PostId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
