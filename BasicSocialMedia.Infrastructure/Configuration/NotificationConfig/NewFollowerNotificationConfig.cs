using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	internal class NewFollowerNotificationConfig : IEntityTypeConfiguration<NewFollowerNotification>
	{
		public void Configure(EntityTypeBuilder<NewFollowerNotification> builder)
		{
			BaseNotificationConfig.ConfigurePostComment(builder);

			builder.HasOne(i => i.UserNotification)
					.WithMany(un => un.NewFollowerNotifications)
					.HasForeignKey(i => i.UserNotificationId)
					.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
