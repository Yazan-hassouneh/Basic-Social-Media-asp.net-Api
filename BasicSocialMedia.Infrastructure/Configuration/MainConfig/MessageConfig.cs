using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.MainConfig
{
	internal class MessageConfig : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			BaseChatMessageConfig.ConfigureChatMessage(builder);
			BaseContentConfig.ConfigureContent(builder);

			builder.Property(model => model.IsRead).IsRequired().HasDefaultValue(false);

			builder.HasOne(message => message.Chat)
				.WithMany(chat => chat.Messages)
				.HasForeignKey(message => message.ChatId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
