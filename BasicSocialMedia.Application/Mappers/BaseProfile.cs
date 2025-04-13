using AutoMapper;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class BaseProfile : Profile

	{
		protected static GetBasicUserInfo MapUser(ApplicationUser? user) =>
		user != null ? new GetBasicUserInfo
		{
			Id = user.Id,
			UserName = user.UserName ?? string.Empty,
			ProfileImage = user?.ProfileImageModel?.Path ?? string.Empty
		}
		: new GetBasicUserInfo();

		protected static List<GetBasicUserInfo> MapReactions<T>(ICollection<T>? reactions) where T : IReactions =>
			reactions?.Select(cr => new GetBasicUserInfo
			{
				Id = cr.UserId,
				UserName = cr.User?.UserName ?? string.Empty,
				ProfileImage = cr.User?.ProfileImageModel!.Path ?? string.Empty
			}).ToList() ?? [];		
		
		protected static List<GetPostFilesDto> MapPostFiles(IEnumerable<PostFileModel>? files) =>
			files?.Select(postFile => new GetPostFilesDto
			{
				Id = postFile.Id,
				UserId = postFile.UserId,
				Path = postFile.Path,
				PostId = postFile.PostId
			}).ToList() ?? [];
	}
}
