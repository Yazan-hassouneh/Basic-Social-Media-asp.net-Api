using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController(IRoleService roleService) : ControllerBase
	{
		private readonly IRoleService _roleService = roleService;

		[HttpPost("addToRole")]
		public async Task<IActionResult> AddToRoleAsync([FromBody] AddRoleDto model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var result = await _roleService.AddToRoleAsync(model);
			if (!string.IsNullOrEmpty(result)) return BadRequest(result);
			return Ok(model);
		}
	}
}
