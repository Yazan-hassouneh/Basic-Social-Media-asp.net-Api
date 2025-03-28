using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.M2MConfig
{
	internal class BlockingConfig : IEntityTypeConfiguration<Block>
	{
		public void Configure(EntityTypeBuilder<Block> builder)
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);

			builder.Property(model => model.BlockerId).IsRequired();
			builder.Property(model => model.BlockedId).IsRequired();

			builder.HasIndex(blocking => new { blocking.BlockerId, blocking.BlockedId }).IsUnique();


			builder.HasOne(blocking => blocking.Blocker)
					.WithMany(user => user.BlockedUsers)
					.HasForeignKey(blocking => blocking.BlockerId)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(blocking => blocking.Blocked)
					.WithMany()
					.HasForeignKey(blocking => blocking.BlockedId)
					.OnDelete(DeleteBehavior.Restrict);


		}
	}
}
