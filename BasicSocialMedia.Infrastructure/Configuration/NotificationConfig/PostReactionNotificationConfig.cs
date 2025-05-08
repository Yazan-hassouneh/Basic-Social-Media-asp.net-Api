using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BasicSocialMedia.Core.Models.Notification;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	internal class PostReactionNotificationConfig : IEntityTypeConfiguration<PostReactionNotification>
	{
		public void Configure(EntityTypeBuilder<PostReactionNotification> builder)
		{
			BaseNotificationConfig.ConfigurePostComment(builder);

			builder.Property(x => x.PostId).IsRequired();
		}
	}
}
