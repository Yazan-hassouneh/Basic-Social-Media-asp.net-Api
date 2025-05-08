using BasicSocialMedia.Core.DTOs.Notification;
using BasicSocialMedia.Core.Models.Notification;

namespace BasicSocialMedia.Application.Mappers.NotificationProfiles
{
	public class NewCommentNotificationProfile : BaseProfile
	{
		public NewCommentNotificationProfile()
		{
			CreateMap<NewCommentNotification, NewCommentNotificationDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.IsRead, opt => opt.MapFrom(src => src.IsRead))
				.ForMember(destination => destination.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(destination => destination.CommentId, opt => opt.MapFrom(src => src.CommentId))
				.ForMember(destination => destination.NotificationType, opt => opt.MapFrom(src => src.NotificationType))
				.ForMember(destination => destination.NotifiedUserId, opt => opt.MapFrom(src => src.NotifiedUserId))
				.ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn));
		}
	}
}
