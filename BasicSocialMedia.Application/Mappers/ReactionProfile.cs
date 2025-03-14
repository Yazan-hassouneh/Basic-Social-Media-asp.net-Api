using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class ReactionProfile : BaseProfile
	{
		public ReactionProfile()
		{
			CreateMap<PostReaction, GetReactionDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)));
		}
	}
}
