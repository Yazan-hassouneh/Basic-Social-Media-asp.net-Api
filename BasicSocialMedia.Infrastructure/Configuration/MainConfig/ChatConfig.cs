using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class ChatConfig : IEntityTypeConfiguration<Chat>
	{
		public void Configure(EntityTypeBuilder<Chat> builder)
		{
			BaseChatMessageConfig.ConfigureChatMessage(builder);

			builder.HasOne(chat => chat.User1)
				.WithMany(user => user.Chats)
				.HasForeignKey(chat => chat.User1Id)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(chat => chat.User2)
				.WithMany()  // A user is also a receiver in multiple chats
				.HasForeignKey(chat => chat.User2Id)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(chat => new { chat.User1Id, chat.User2Id }).IsUnique();
		}
	}
}
