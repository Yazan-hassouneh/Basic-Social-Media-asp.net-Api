using BasicSocialMedia.Core.DTOs.EnumsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;
using static BasicSocialMedia.Core.Enums.ProjectEnums;

namespace BasicSocialMedia.Application.Services.EnumsServices
{
	public class AudienceService : IAudienceService
	{
		public IEnumerable<AudienceDto> GetAudiences()
		{
			// Example data, replace with actual data retrieval logic
			IEnumerable<AudienceDto> audiences = Enum.GetValues<PostAudience>()
														.Cast<PostAudience>()
														.Select(Audience => new AudienceDto { Name = Audience.ToString(), Value = (int)Audience })
														.ToList();
			return audiences;
		}
	}
}
