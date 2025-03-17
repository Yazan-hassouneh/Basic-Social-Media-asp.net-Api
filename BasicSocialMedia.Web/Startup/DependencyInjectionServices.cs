using BasicSocialMedia.Application.Services.AuthServices;
using BasicSocialMedia.Application.Services.EnumsServices;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;

namespace BasicSocialMedia.Web.Startup
{
	public static class DependencyInjectionServices
	{
		internal static IServiceCollection AddServicesInjection(this IServiceCollection services)
		{
			services.AddScoped<IJWTService, JWTService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IChatServices, ChatService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<IAudienceService, AudienceService>();
			services.AddScoped<IMessagesServices, MessageService>();
			services.AddScoped<IPostReactionService, PostReactionService>();
			services.AddScoped<ICommentReactionService, CommentReactionService>();
			return services;
		}		

	}
}
