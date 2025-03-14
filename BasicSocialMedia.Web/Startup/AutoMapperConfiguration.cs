using BasicSocialMedia.Application.Mappers;

namespace BasicSocialMedia.Web.Startup
{
	public static class AutoMapperConfiguration
	{
		internal static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(CommentProfile));
			services.AddAutoMapper(typeof(PostProfile));
			services.AddAutoMapper(typeof(ReactionProfile));
			return services;
		}
	}
}
