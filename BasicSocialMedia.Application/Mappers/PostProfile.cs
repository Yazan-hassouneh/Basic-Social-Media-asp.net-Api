﻿using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class PostProfile : BasePostCommentProfile
	{
		public PostProfile() 
		{
			CreateMap<Post, GetPostDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User, opt => opt.MapFrom(src => MapUser(src.User)))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.ReactionsCount, opt => opt.MapFrom(src => src.PostReactions != null ? src.PostReactions.Count : 0))
				.ForMember(destination => destination.CommentsCount, opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0))
				.ForMember(destination => destination.ReactionsList, opt => opt.MapFrom(src => MapReactions(src.PostReactions)))
				.ReverseMap();
		}
	}
}
