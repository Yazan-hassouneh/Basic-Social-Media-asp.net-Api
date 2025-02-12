using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class CommentReactionConfig : IEntityTypeConfiguration<CommentReaction>
	{
		public void Configure(EntityTypeBuilder<CommentReaction> builder)
		{
			BaseReactionsConfig.ConfigureReaction(builder);

			builder.HasOne(reaction => reaction.Comment)
						.WithMany(post => post.CommentReactions)
						.HasForeignKey(reaction => reaction.CommentId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
