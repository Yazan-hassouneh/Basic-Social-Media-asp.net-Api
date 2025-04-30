using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.MessagingConfig
{
	internal class MessageConfig : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);
			BaseContentConfig.ConfigureContent(builder);

			builder.Property(i => i.SenderId).IsRequired(false);
			builder.Property(i => i.ReceiverId).IsRequired(false);

			builder.Property(model => model.IsRead).IsRequired().HasDefaultValue(false);

			builder.HasOne(message => message.Chat)
				.WithMany(chat => chat.Messages)
				.HasForeignKey(message => message.ChatId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
