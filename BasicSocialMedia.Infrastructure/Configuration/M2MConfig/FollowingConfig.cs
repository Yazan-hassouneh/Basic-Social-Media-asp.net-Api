using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.M2MConfig
{
	internal class FollowingConfig : IEntityTypeConfiguration<Follow>
	{
		public void Configure(EntityTypeBuilder<Follow> builder)
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);
			builder.Property(model => model.FollowerId).IsRequired(false);
			builder.Property(model => model.FollowingId).IsRequired(false);


			builder.HasOne(Follow => Follow.Follower)
					.WithMany(user => user.Followers)
					.HasForeignKey(Follow => Follow.FollowerId)
					.OnDelete(DeleteBehavior.Restrict);			
			
			builder.HasOne(Follow => Follow.FollowingUser)
					.WithMany(user => user.Following)
					.HasForeignKey(Follow => Follow.FollowingId)
					.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
