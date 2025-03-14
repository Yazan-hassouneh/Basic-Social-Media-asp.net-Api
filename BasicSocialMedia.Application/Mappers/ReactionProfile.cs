using AutoMapper;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class ReactionProfile : Profile
	{
		public ReactionProfile()
		{
			CreateMap<PostReaction, GetReactionDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => new GetBasicUserInfo
					{
						Id = src.UserId,
						UserName = src.User == null ? string.Empty : src.User.UserName ?? string.Empty
					})
				);
		}
	}
}
