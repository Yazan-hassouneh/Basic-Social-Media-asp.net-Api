using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicSocialMedia.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = PoliciesSettings.allowAllUsersPolicy)]
	public class AudienceController(IAudienceService audienceService) : ControllerBase
	{
		private readonly IAudienceService _audienceService = audienceService;

		[HttpGet("getAll")]
		public IActionResult GetAudiences()
		{
			var audiences = _audienceService.GetAudiences();
			return Ok(audiences);
		}
	}
}
