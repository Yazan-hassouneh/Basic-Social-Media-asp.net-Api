using BasicSocialMedia.Core.DTOs.EnumsDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices
{
	public interface IAudienceService
	{
		IEnumerable<AudienceDto> GetAudiencesAsync();
	}
}
