using BasicSocialMedia.Application.Helpers;
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

			builder.Services.AddIdentityServices();
			builder.Services.AddJWTServices(builder);
            builder.Services.AddServicesInjection();
            builder.Services.AddAutoMapperConfiguration();


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
