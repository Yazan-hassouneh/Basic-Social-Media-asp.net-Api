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

			builder.Property(model => model.SenderId).IsRequired();
			builder.Property(model => model.ReceiverId).IsRequired();
			builder.HasIndex(friendship => new { friendship.SenderId, friendship.ReceiverId }).IsUnique();


			builder.HasOne(friendship => friendship.Sender)
					.WithMany(user => user.Friendships)
					.HasForeignKey(friendship => friendship.SenderId)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(friendship => friendship.Receiver)
					.WithMany()
					.HasForeignKey(friendship => friendship.ReceiverId)
					.OnDelete(DeleteBehavior.Restrict);


		}
	}
}
