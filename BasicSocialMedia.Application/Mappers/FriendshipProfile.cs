using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Mappers
{
	public class FriendshipProfile : BaseProfile
	{
		public FriendshipProfile()
		{
			CreateMap<Friendship, GetFriendsDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.FriendId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.SenderId) ? src.ReceiverId : src.SenderId))
				.ForMember(destination => destination.Friend, opt => opt.MapFrom(src => MapUser(src.Sender ?? src.Receiver)));

		}
	}
}
