using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Mappers
{
	public class BlockProfile : BaseProfile
	{
		public BlockProfile()
		{
			CreateMap<Block, GetBlockUserDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.BlockedId, opt => opt.MapFrom(src => src.BlockedId))
				.ForMember(destination => destination.BlockerId, opt => opt.MapFrom(src => src.BlockerId))
				.ForMember(destination => destination.Blocked, opt => opt.MapFrom(src => MapUser(src.Blocked)));

		}
	}
}
