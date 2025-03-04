using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	internal class RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IRoleService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly RoleManager<IdentityRole> _roleManager = roleManager;
		public async Task<string> AddToRoleAsync(AddRoleDto model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName)) return "UserId or Role Name Invalid";
			if (await _userManager.IsInRoleAsync(user, model.RoleName)) return "User already assigned to the role";

			var result = await _userManager.AddToRoleAsync(user, model.RoleName);
			return result.Succeeded ? "" : "Something went wrong";
		}
	}
}
