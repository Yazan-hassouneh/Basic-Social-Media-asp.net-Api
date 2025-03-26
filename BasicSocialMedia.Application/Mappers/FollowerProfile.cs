using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Mappers
{
	public class FollowerProfile : BaseProfile
	{
		public FollowerProfile()
		{
			CreateMap<Follow, GetFollowerDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.FollowerId, opt => opt.MapFrom(src => src.FollowerId))
				.ForMember(destination => destination.Follower, opt => opt.MapFrom(src => MapUser(src.Follower)));

		}
	}
}
