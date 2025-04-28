using Hangfire;

namespace BasicSocialMedia.Web.Startup
{
	internal static class BackgroundJobsServices
	{
		internal static IServiceCollection AddHangFireConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("AppConnectionString");
			services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
			services.AddHangfireServer();

			/* Important to add this line in Program.cs  

				app.UseHangfireDashboard("/dashboardUrl");  
*/
			return services;
		}
	}
}
