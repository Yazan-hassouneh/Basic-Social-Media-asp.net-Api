using BasicSocialMedia.Core.DTOs.EnumsDTOs;
using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AudienceController(IAudienceService audienceService) : ControllerBase
	{
		private readonly IAudienceService _audienceService = audienceService;

		[HttpGet("audiences")]
		public IActionResult GetAudiences()
		{
			var audiences = _audienceService.GetAudiences();
			return Ok(audiences);
		}
	}
}
