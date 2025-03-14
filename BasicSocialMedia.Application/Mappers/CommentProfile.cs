using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class CommentProfile : BaseProfile
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
	}
}
