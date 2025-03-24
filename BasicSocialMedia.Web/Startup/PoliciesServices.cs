using BasicSocialMedia.Core.Consts;

namespace BasicSocialMedia.Web.Startup
{
	public static class PoliciesServices
	{
		internal static IServiceCollection AddPoliciesServices(this IServiceCollection services)
		{
			services.AddAuthorizationBuilder()
				.AddPolicy(PoliciesSettings.allowAllUsersPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.superAdminRoleName, RolesSettings.adminRoleName, RolesSettings.userRoleName);
				})	
				
				.AddPolicy(PoliciesSettings.allowSuperAdminAdminPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.adminRoleName, RolesSettings.superAdminRoleName);
				})

				.AddPolicy(PoliciesSettings.allowOnlySuperAdminPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.superAdminRoleName);
				});

			//.AddPolicy("AdminUserPolicy", options =>
			//{
			//	options.RequireAuthenticatedUser();
			//	options.RequireRole("admin", "user");
			//	//options.Requirements.Add(new MinimumAgeRequirement(18));
			//});

			return services;
		}
	}
}
