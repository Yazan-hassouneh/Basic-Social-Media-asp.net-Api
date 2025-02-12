using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class PostReactionConfig : IEntityTypeConfiguration<PostReaction>
	{
		public void Configure(EntityTypeBuilder<PostReaction> builder)
		{
			BaseReactionsConfig.ConfigureReaction(builder);

			builder.HasOne(reaction => reaction.Post)
						.WithMany(post => post.PostReactions)
						.HasForeignKey(reaction => reaction.PostId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
