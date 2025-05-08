using BasicSocialMedia.Core.DTOs.Notification;
using BasicSocialMedia.Core.Models.Notification;

namespace BasicSocialMedia.Application.Mappers.NotificationProfiles
{
	public class FriendRequestNotificationProfile : BaseProfile
	{
		public FriendRequestNotificationProfile()
		{
			CreateMap<FriendRequestNotification, FriendRequestNotificationDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.IsRead, opt => opt.MapFrom(src => src.IsRead))
				.ForMember(destination => destination.NotificationType, opt => opt.MapFrom(src => src.NotificationType))
				.ForMember(destination => destination.NotifiedUserId, opt => opt.MapFrom(src => src.NotifiedUserId))
				.ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn));
		}
	}
}
