using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	public static class BaseIdConfig
	{
		public static void ConfigureId<T>(EntityTypeBuilder<T> builder) where T : class, IId
		{
			builder.HasKey(i => i.Id);
			builder.Property(i => i.Id).UseIdentityColumn();
		}
	}
}
