using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Infrastructure.Configuration.MessagingConfig
{
	internal class DeletedMessageConfig : IEntityTypeConfiguration<DeletedMessage>
	{
		public void Configure(EntityTypeBuilder<DeletedMessage> builder)
		{
			BaseIdConfig.ConfigureId(builder);

			builder.Property(i => i.MessageId).IsRequired();
			builder.Property(i => i.UserId).IsRequired();
			builder.HasIndex(dm => new { dm.MessageId, dm.UserId }).IsUnique();

			builder.HasOne(dm => dm.Message)
				.WithMany(m => m.DeletedByUsers)
				.HasForeignKey(dm => dm.MessageId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(dm => dm.Chat)
				.WithMany()
				.HasForeignKey(dm => dm.ChatId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
