using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.NotificationConfig
{
	public class UserNotificationConfig : IEntityTypeConfiguration<UserNotification>
	{
		public void Configure(EntityTypeBuilder<UserNotification> builder)
		{
			BaseIdConfig.ConfigureId(builder);

			builder.Property(x => x.UserId).IsRequired();

			builder.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}