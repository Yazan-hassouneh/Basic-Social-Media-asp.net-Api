using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	internal class NewCommentNotificationConfig : IEntityTypeConfiguration<NewCommentNotification>
	{
		public void Configure(EntityTypeBuilder<NewCommentNotification> builder)
		{
			BaseNotificationConfig.ConfigurePostComment(builder);

			builder.Property(x => x.PostId).IsRequired();
			builder.Property(x => x.CommentId).IsRequired();

			builder.HasOne(i => i.UserNotification)
					.WithMany(un => un.NewCommentNotifications)
					.HasForeignKey(i => i.UserNotificationId)
					.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
