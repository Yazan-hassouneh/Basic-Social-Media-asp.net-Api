using AngleSharp.Css;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController(IRoleService roleService, IValidator<AddRoleDto> addRoleDtoValidator, IValidator<AddToRoleDto> addToRoleDtoValidator) : ControllerBase
	{
		private readonly IRoleService _roleService = roleService;
		private readonly IValidator<AddRoleDto> _addRoleDtoValidator = addRoleDtoValidator;
		private readonly IValidator<AddToRoleDto> _addToRoleDtoValidator = addToRoleDtoValidator;

		[HttpPost("assignRole")]
		public async Task<IActionResult> AssignRole([FromBody] AddToRoleDto model)
		{
			if (!Enum.TryParse(typeof(AllowedRoles), model.RoleName, true, out _))
			{
				return BadRequest("Role Not Allowed.");
			}

			var validationResult = await _addToRoleDtoValidator.ValidateAsync(model);
			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

			var result = await _roleService.AddToRoleAsync(model);
			if (!string.IsNullOrEmpty(result)) return BadRequest(result);
			return Ok(model);
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddRole([FromBody] AddRoleDto model)
		{
			if (!Enum.TryParse(typeof(AllowedRoles), model.RoleName, true, out _))
			{
				return BadRequest("Role Not Allowed.");
			}

			var validationResult = await _addRoleDtoValidator.ValidateAsync(model);
			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

			var result = await _roleService.AddNewRoleAsync(model);
			if (!string.IsNullOrEmpty(result)) return BadRequest(result);
			return Ok(model);
		}
		
		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteRole([FromBody] AddRoleDto model)
		{
			if (Enum.TryParse(typeof(NotAllowedRolesToDelete), model.RoleName, true, out _))
			{
				return BadRequest("You cannot delete this role.");
			}

			var validationResult = await _addRoleDtoValidator.ValidateAsync(model);
			if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

			var result = await _roleService.DeleteRoleAsync(model);
			if (!string.IsNullOrEmpty(result)) return BadRequest(result);
			return Ok(model);
		}
	}
}
