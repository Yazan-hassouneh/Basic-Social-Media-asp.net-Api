using BasicSocialMedia.Application.Mappers;
using BasicSocialMedia.Application.Mappers.NotificationProfiles;

namespace BasicSocialMedia.Web.Startup
{
	public static class AutoMapperConfiguration
	{
		internal static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(PostProfile));
			services.AddAutoMapper(typeof(ChatProfile));
			services.AddAutoMapper(typeof(BlockProfile));
			services.AddAutoMapper(typeof(MessageProfile));
			services.AddAutoMapper(typeof(CommentProfile));
			services.AddAutoMapper(typeof(ReactionProfile));
			services.AddAutoMapper(typeof(FollowerProfile));
			services.AddAutoMapper(typeof(FollowingProfile));
			services.AddAutoMapper(typeof(NewCommentNotificationProfile));
			services.AddAutoMapper(typeof(NewFollowerNotificationProfile));
			services.AddAutoMapper(typeof(PostReactionNotificationProfile));
			services.AddAutoMapper(typeof(FriendRequestNotificationProfile));
			services.AddAutoMapper(typeof(CommentReactionNotificationProfile));
			return services;
		}
	}
}
