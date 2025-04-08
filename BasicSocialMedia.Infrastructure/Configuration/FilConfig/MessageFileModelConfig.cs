using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.FilConfig
{
	internal class MessageFileModelConfig : IEntityTypeConfiguration<MessageFileModel>
	{
		public void Configure(EntityTypeBuilder<MessageFileModel> builder)
		{
			BaseFileModelConfig.ConfigurePostComment(builder);

			builder.Property(model => model.MessageId).IsRequired();
			builder.Property(model => model.ChatId).IsRequired();


			builder.HasOne(messageFile => messageFile.Message)
						.WithMany(message => message.Files)
						.HasForeignKey(messageFile => messageFile.MessageId)
						.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(chatFile => chatFile.Chat)
						.WithMany(chat => chat.Files)
						.HasForeignKey(chatFile => chatFile.MessageId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
