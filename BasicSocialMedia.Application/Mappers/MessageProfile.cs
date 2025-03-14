using AutoMapper;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class MessageProfile : Profile
	{
		public MessageProfile()
		{
			CreateMap<Message, GetMessagesDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User1Id, opt => opt.MapFrom(src => src.User1Id))
				.ForMember(destination => destination.User2Id, opt => opt.MapFrom(src => src.User2Id))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(destination => destination.IsRead, opt => opt.MapFrom(src => src.IsRead))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ReverseMap();
		}
	}
}
