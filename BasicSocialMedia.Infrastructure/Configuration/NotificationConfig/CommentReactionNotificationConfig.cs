using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	internal class CommentReactionNotificationConfig : IEntityTypeConfiguration<CommentReactionNotification>
	{
		public void Configure(EntityTypeBuilder<CommentReactionNotification> builder)
		{
			BaseNotificationConfig.ConfigurePostComment(builder);

			builder.Property(x => x.PostId).IsRequired();
			builder.Property(x => x.CommentId).IsRequired();

			builder.HasOne(i => i.UserNotification)
					.WithMany(un => un.CommentReactionNotifications)
					.HasForeignKey(i => i.UserNotificationId)
					.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
