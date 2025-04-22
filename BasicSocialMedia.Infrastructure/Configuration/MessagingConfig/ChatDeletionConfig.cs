using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.MessagingConfig
{
	internal class ChatDeletionConfig : IEntityTypeConfiguration<ChatDeletion>
	{
		public void Configure(EntityTypeBuilder<ChatDeletion> builder)
		{
			BaseIdConfig.ConfigureId(builder);

			builder.Property(i => i.UserId).IsRequired();
			builder.HasIndex(dm => new { dm.ChatId, dm.UserId }).IsUnique();
		}
	}
}
