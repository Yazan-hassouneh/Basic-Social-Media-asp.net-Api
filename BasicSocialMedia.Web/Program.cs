using BasicSocialMedia.Application.Helpers;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Web.Startup;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
			builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
		             builder.Configuration.GetConnectionString("AppConnectionString")
	             ));

            builder.Services.AddUnitOfWorkInjection();
			builder.Services.AddIdentityServices();
			builder.Services.AddJWTServices(builder);
            builder.Services.AddPoliciesServices();
			builder.Services.AddCorsPolicies();
            builder.Services.AddServicesInjection();
            builder.Services.AddHtmlSanitizerInjection();
            builder.Services.AddAutoMapperConfiguration();
            builder.Services.AddDTOsValidatorsInjection();


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

			// Enable serving static files from wwwroot
			app.UseStaticFiles();

			app.UseHttpsRedirection();

			app.UseCors(CorsSettings.allowAllOrigins);
			//app.UseCors(CorsSettings.allowSpecificOrigins);

			// Important to add 
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

            app.Run();
        }
    }
}
