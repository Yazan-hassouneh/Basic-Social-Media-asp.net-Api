using AngleSharp.Css;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IRoleService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly RoleManager<IdentityRole> _roleManager = roleManager;

		public async Task<string> AddToRoleAsync(AddToRoleDto model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName)) return "UserId or Role Name Invalid";
			if (await _userManager.IsInRoleAsync(user, model.RoleName)) return "User already assigned to the role";

			var result = await _userManager.AddToRoleAsync(user, model.RoleName);
			return result.Succeeded ? "" : ValidationSettings.GeneralErrorMessage;
		}
		public async Task<string> AddNewRoleAsync(AddRoleDto model)
		{
			var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
			if (roleExists) return "Role already exists";
			
			var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName.Trim()));

			return result.Succeeded ? "" : ValidationSettings.GeneralErrorMessage;

		}
		public async Task<string?> DeleteRoleAsync(AddRoleDto roleDto)
		{
			var role = await _roleManager.FindByNameAsync(roleDto.RoleName);
			if (role == null) return ValidationSettings.GeneralErrorMessage;

			var usersInRole = await _userManager.GetUsersInRoleAsync(roleDto.RoleName);
			foreach (var user in usersInRole)
			{
				await _userManager.RemoveFromRoleAsync(user, roleDto.RoleName);
			}

			var result = await _roleManager.DeleteAsync(role);
			return result.Succeeded ? "" : ValidationSettings.GeneralErrorMessage;
		}
	}
}
