﻿using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicSocialMedia.Infrastructure.Configuration.BaseConfig
{
	internal static class BasePostCommentConfig
	{
		public static void ConfigurePostComment<T>(EntityTypeBuilder<T> builder) where T : class, IPostComment
		{
			BaseIdConfig.ConfigureId(builder);
			BaseTimestampConfig.ConfigureTimestamp(builder);
			BaseContentConfig.ConfigureContent(builder);
			builder.Property(i => i.UserId).IsRequired();
			builder.Property(i => i.LikesCount).IsRequired().HasDefaultValue(ModelsSettings.LikesCountMin);
			builder.Property(i => i.IsDeleted).IsRequired().HasDefaultValue(false);

		}
	}
}
