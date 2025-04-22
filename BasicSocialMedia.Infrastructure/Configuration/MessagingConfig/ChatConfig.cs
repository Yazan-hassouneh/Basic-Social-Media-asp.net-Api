using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.MessagingConfig
{
	internal class ChatConfig : IEntityTypeConfiguration<Chat>
	{
		public void Configure(EntityTypeBuilder<Chat> builder)
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);

			builder.Property(i => i.User1Id).IsRequired();
			builder.Property(i => i.User2Id).IsRequired();

			builder.HasOne(chat => chat.User1)
				.WithMany(user => user.Chats)
				.HasForeignKey(chat => chat.User1Id)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(chat => chat.User2)
				.WithMany()  // A user is also a receiver in multiple chats
				.HasForeignKey(chat => chat.User2Id)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
