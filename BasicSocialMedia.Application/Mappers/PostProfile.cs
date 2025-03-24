using BasicSocialMedia.Application.Utils;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Models.MainModels;
using System.Net;

namespace BasicSocialMedia.Application.Mappers
{
	public class PostProfile : BaseProfile
	{
		public PostProfile()
		{
			CreateMap<Post, GetPostDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => WebUtility.HtmlEncode(src.Content)))
				.ForMember(destination => destination.Audience, opt => opt.MapFrom(src => src.Audience))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.ReactionsCount, opt => opt.MapFrom(src => src.PostReactions != null ? src.PostReactions.Count : 0))
				.ForMember(destination => destination.CommentsCount, opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0))
				.ForMember(destination => destination.ReactionsList, opt => opt.MapFrom(src => MapReactions(src.PostReactions)))
				.ForMember(destination => destination.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));

			CreateMap<GetPostDto, Post>()
				.ForMember(destination => destination.RowVersion, opt => opt.ConvertUsing<Base64StringToByteArrayConverter, string>(src => src.RowVersion));

		}
	}
}
