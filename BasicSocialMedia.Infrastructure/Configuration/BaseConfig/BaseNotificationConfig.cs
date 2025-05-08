using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal class BaseNotificationConfig
	{
		public static void ConfigurePostComment<T>(EntityTypeBuilder<T> builder) where T : class, INotification
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);

			builder.Property(i => i.UserId).IsRequired();
			builder.Property(i => i.NotificationType).IsRequired();
			builder.Property(i => i.NotifiedUserId).IsRequired();
			builder.Property(i => i.IsRead).IsRequired().HasDefaultValue(false);

			builder.HasOne(i => i.User)
				   .WithMany()
				   .HasForeignKey(i => i.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

		}
	}
}
