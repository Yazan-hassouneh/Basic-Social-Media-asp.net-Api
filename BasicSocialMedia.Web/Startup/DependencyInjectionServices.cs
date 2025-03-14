using BasicSocialMedia.Application.Services.AuthServices;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;

namespace BasicSocialMedia.Web.Startup
{
	public static class DependencyInjectionServices
	{
		internal static IServiceCollection AddServicesInjection(this IServiceCollection services)
		{
			services.AddScoped<IJWTService, JWTService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ICommentService, CommentService>();
			return services;
		}		

	}
}
