using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal static class BaseReactionsConfig
	{
		public static void ConfigureReaction<T>(EntityTypeBuilder<T> builder) where T : class, IReactions
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder); 
			builder.Property(i => i.UserId).IsRequired();

			builder.HasOne(reaction => reaction.User)
				.WithMany()
				.HasForeignKey(reaction => reaction.UserId)
				.HasPrincipalKey(user => user.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
