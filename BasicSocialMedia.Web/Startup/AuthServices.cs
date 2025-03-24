using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BasicSocialMedia.Web.Startup
{
	public static class AuthServices
	{
		internal static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			// Important to add in Program.cs
			// app.UseAuthentication();
			// app.UseAuthorization();

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager()
				.AddRoles<IdentityRole>();

			return services;
		}
		internal static IServiceCollection AddJWTServices(this IServiceCollection services, WebApplicationBuilder builder)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(jwtOptions =>
			{
				jwtOptions.RequireHttpsMetadata = false;
				jwtOptions.SaveToken = false;
				jwtOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidAudience = builder.Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"])),
					ClockSkew = TimeSpan.Zero,
				};
			});

			return services;
		}
		internal static void AddCorsPolicies(this IServiceCollection services)
		{
			// For Development Only
			services.AddCors(options =>
			{
				// for Development
				options.AddPolicy(CorsSettings.allowAllOrigins, policy =>
					{
						policy.AllowAnyOrigin()
							  .AllowAnyMethod()
							  .AllowAnyHeader();
					}
				);

				
				//for production 
				//options.AddPolicy(CorsSettings.allowSpecificOrigins, policy =>
				//	{
				//		policy.WithOrigins("https://example.com") // Replace with your frontend URL
				//			  .AllowAnyMethod()
				//			  .AllowAnyHeader();
				//	}
				//);
				
			});

			/*
				-- Important :-

				Enable CORS in Program.cs
				app.UseCors(CorsSettings.allowAllOrigins);
			 */
		}
	}
}
