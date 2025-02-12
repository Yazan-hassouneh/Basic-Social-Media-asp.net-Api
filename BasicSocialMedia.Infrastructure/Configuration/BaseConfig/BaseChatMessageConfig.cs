using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal static class BaseChatMessageConfig
	{
		public static void ConfigureChatMessage<T>(EntityTypeBuilder<T> builder) where T : class, IChatMessage
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);

			builder.Property(i => i.User1Id).IsRequired();
			builder.Property(i => i.User2Id).IsRequired();

			
		}
	}
}
