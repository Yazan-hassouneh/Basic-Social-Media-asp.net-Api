using BasicSocialMedia.Core.DTOs.EnumsDTOs;
using BasicSocialMedia.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AudienceController : ControllerBase
	{
		[HttpGet("audiences")]
		public IActionResult GetAudiences()
		{
			var audiences = Enum.GetValues<ProjectEnums.PostAudience>()
								.Cast<ProjectEnums.PostAudience>()
								.Select(audience => new AudienceDto { Name = audience.ToString(), Value = (int)audience })
								.ToList();

			return Ok(audiences);
		}
	}
}
