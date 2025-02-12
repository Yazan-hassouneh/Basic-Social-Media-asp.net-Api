using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal static class BaseContentConfig
	{
		public static void ConfigureContent<T>(EntityTypeBuilder<T> builder) where T : class, IContent
		{
			builder.Property(i => i.Content).IsRequired(true);
		}
	}
}
