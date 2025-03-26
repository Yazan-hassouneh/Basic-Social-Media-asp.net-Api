using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Mappers
{
	public class FollowingProfile : BaseProfile
	{
		public FollowingProfile()
		{
			CreateMap<Follow, GetFollowingDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.FollowingId, opt => opt.MapFrom(src => src.FollowingId))
				.ForMember(destination => destination.FollowingUser, opt => opt.MapFrom(src => MapUser(src.FollowingUser)));

		}
	}
}
