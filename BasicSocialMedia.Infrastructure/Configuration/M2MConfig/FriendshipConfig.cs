using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Infrastructure.Configuration.M2MConfig
{
	internal class FriendshipConfig : IEntityTypeConfiguration<Friendship>
	{
		public void Configure(EntityTypeBuilder<Friendship> builder)
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);
			builder.Property(model => model.Status)
				.HasConversion<int>() // Store as integer
				.IsRequired()
				.HasDefaultValue(FriendshipStatus.Pending); // Default as integer

			builder.Property(model => model.UserId1).IsRequired();
			builder.Property(model => model.UserId2).IsRequired();
			builder.HasIndex(friendship => new { friendship.UserId1, friendship.UserId2 }).IsUnique();


			builder.HasOne(friendship => friendship.User1)
					.WithMany(user => user.Friendships)
					.HasForeignKey(friendship => friendship.UserId1)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(friendship => friendship.User2)
					.WithMany(user => user.Friendships)
					.HasForeignKey(friendship => friendship.UserId2)
					.OnDelete(DeleteBehavior.Restrict);


		}
	}
}
