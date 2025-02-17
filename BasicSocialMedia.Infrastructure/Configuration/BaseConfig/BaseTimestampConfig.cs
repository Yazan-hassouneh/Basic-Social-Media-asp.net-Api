using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal static class BaseTimestampConfig
	{
		public static void ConfigureTimestamp<T>(EntityTypeBuilder<T> builder) where T : class, ITimestamp
		{
			builder.Property(i => i.CreatedOn).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
		}
	}
}
