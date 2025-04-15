using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.MainModels;
using System.Net;

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
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => WebUtility.HtmlEncode(src.Content)))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => MapCommentFiles(src.Files)))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
				.ForMember(destination => destination.ReactionsCount, opt => opt.MapFrom(src => src.CommentReactions != null ? src.CommentReactions.Count : 0))
				.ForMember(destination => destination.ReactionsList, opt => opt.MapFrom(src => MapReactions(src.CommentReactions)));		
		}

		protected static List<GetCommentFileDto> MapCommentFiles(IEnumerable<CommentFileModel>? files) =>
			files?.Select(postFile => new GetCommentFileDto
			{
				Id = postFile.Id,
				UserId = postFile.UserId,
				Path = postFile.Path,
				PostId = postFile.PostId,
				CommentId = postFile.CommentId,
			}).ToList() ?? [];
	}
}
