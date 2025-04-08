using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal class BaseFileModelConfig
	{
		public static void ConfigurePostComment<T>(EntityTypeBuilder<T> builder) where T : class, IFileModel
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);

			builder.Property(model => model.UserId).IsRequired();
			builder.Property(model => model.Path).IsRequired();
			builder.Property(model => model.RowVersion).IsRowVersion();

		}
	}
}
