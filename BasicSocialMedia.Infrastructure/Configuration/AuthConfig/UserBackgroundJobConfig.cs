using BasicSocialMedia.Infrastructure.Configuration.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Infrastructure.Configuration.AuthConfig
{
	internal class UserBackgroundJobConfig : IEntityTypeConfiguration<UserBackgroundJob>
	{
		public void Configure(EntityTypeBuilder<UserBackgroundJob> builder)
		{
			BaseIdConfig.ConfigureId(builder);

			builder.Property(model => model.UserId).IsRequired();
			builder.Property(model => model.JobId).IsRequired();
			builder.Property(model => model.JobType).IsRequired();
			// Add unique constraint to prevent duplicate UserId and JobId  
			builder.HasIndex(model => new { model.UserId, model.JobId }).IsUnique();
			builder.Property(model => model.ScheduledAt).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");

		}
	}
}
