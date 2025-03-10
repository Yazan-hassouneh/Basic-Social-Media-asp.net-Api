using BasicSocialMedia.Application.Services.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;

namespace BasicSocialMedia.Web.Startup
{
	public static class DependencyInjectionServices
	{
		internal static IServiceCollection AddServicesInjection(this IServiceCollection services)
		{
			services.AddScoped<IJWTService, JWTService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IRoleService, RoleService>();
			return services;
		}		

	}
}
