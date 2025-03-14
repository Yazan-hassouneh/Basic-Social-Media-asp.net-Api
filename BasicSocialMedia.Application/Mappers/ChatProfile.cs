using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class ChatProfile : BaseProfile
	{
		public ChatProfile() 
		{
			CreateMap<Chat, GetChatDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User1Id, opt => opt.MapFrom(src => src.User1Id))
				.ForMember(destination => destination.User2Id, opt => opt.MapFrom(src => src.User2Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.User1, opt => opt.MapFrom(src => MapUser(src.User1)))
				.ForMember(destination => destination.User2, opt => opt.MapFrom(src => MapUser(src.User2)))
				.ReverseMap();
		}
	}
}
