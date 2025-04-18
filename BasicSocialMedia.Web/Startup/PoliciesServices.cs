using BasicSocialMedia.Application.CustomPolicies.Block;
using BasicSocialMedia.Application.CustomPolicies.Moderation;
using BasicSocialMedia.Application.CustomPolicies.Ownership;
using BasicSocialMedia.Application.CustomPolicies.PostVisibility;
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
					options.RequireRole(RolesSettings.superAdminRoleName, RolesSettings.adminRoleName, RolesSettings.userRoleName, RolesSettings.ModeratorRoleName);
				})

				.AddPolicy(PoliciesSettings.allowSuperAdminAdminPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.adminRoleName, RolesSettings.superAdminRoleName);
				})

				.AddPolicy(PoliciesSettings.allowSuperAdminAdminModeratorPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.superAdminRoleName, RolesSettings.ModeratorRoleName, RolesSettings.userRoleName);
				})

				.AddPolicy(PoliciesSettings.allowOnlySuperAdminPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.superAdminRoleName);
				})

				.AddPolicy(PoliciesSettings.CanDeletePost, options =>
				 {
					 options.RequireAuthenticatedUser();
					 options.RequireRole(RolesSettings.superAdminRoleName, RolesSettings.ModeratorRoleName, RolesSettings.userRoleName);
					 options.Requirements.Add(new PostModerationRequirement("Delete"));
				 })

				.AddPolicy(PoliciesSettings.CanEditPost, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.superAdminRoleName, RolesSettings.ModeratorRoleName, RolesSettings.userRoleName);
					options.Requirements.Add(new PostModerationRequirement("Edit"));
				})

				.AddPolicy(PoliciesSettings.Ownership, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.userRoleName);
					options.Requirements.Add(new OwnershipRequirement());
				})

				.AddPolicy(PoliciesSettings.PostVisibilityPolicy, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.userRoleName);
					options.Requirements.Add(new PostVisibilityRequirement());
				})
				
				.AddPolicy(PoliciesSettings.IsUserBlocked, options =>
				{
					options.RequireAuthenticatedUser();
					options.RequireRole(RolesSettings.userRoleName);
					options.Requirements.Add(new BlockedRequirement());
				});

			return services;
		}
	}
}
