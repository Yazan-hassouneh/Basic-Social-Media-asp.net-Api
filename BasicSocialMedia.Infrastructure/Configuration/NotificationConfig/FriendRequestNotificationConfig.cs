using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	internal class FriendRequestNotificationConfig : IEntityTypeConfiguration<FriendRequestNotification>
	{
		public void Configure(EntityTypeBuilder<FriendRequestNotification> builder)
		{
			BaseNotificationConfig.ConfigurePostComment(builder);

			builder.HasOne(i => i.UserNotification)
					.WithMany(un => un.FriendRequestNotifications)
					.HasForeignKey(i => i.UserNotificationId)
					.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
