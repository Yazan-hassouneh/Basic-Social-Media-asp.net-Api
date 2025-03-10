using AutoMapper;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class CommentProfile : Profile
	{
		public CommentProfile()
		{
			CreateMap<Comment, GetCommentDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)))
				.ForMember(destination => destination.PostId, opt => opt.MapFrom(src => src.PostId))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.ReactionsCount, opt => opt.MapFrom(src => src.CommentReactions != null ? src.CommentReactions.Count : 0))
				.ForMember(destination => destination.ReactionsList, opt => opt.MapFrom(src => MapReactions(src.CommentReactions)))
				.ReverseMap();
		}
		private static GetBasicUserInfo MapUser(ApplicationUser ? user) =>
			user != null ? new GetBasicUserInfo
			{
				Id = user.Id,
				UserName = user.UserName ?? string.Empty,
				ProfileImage = user.ProfileImage ?? string.Empty
			}
			: new GetBasicUserInfo();

		private static List<GetBasicUserInfo> MapReactions(ICollection<CommentReaction>? reactions) =>
			reactions?.Select(cr => new GetBasicUserInfo
			{
				Id = cr.UserId,
				UserName = cr.User?.UserName ?? string.Empty,
				ProfileImage = cr.User?.ProfileImage ?? string.Empty
			}).ToList() ?? [];
	}
}
