using AutoMapper;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Application.Mappers
{
	public class BaseProfile : Profile

	{
		protected static GetBasicUserInfo MapUser(ApplicationUser? user) =>
		user != null ? new GetBasicUserInfo
		{
			Id = user.Id,
			UserName = user.UserName ?? string.Empty,
			ProfileImage = user.ProfileImage ?? string.Empty
		}
		: new GetBasicUserInfo();

		protected static List<GetBasicUserInfo> MapReactions<T>(ICollection<T>? reactions) where T : IReactions =>
			reactions?.Select(cr => new GetBasicUserInfo
			{
				Id = cr.UserId,
				UserName = cr.User?.UserName ?? string.Empty,
				ProfileImage = cr.User?.ProfileImage ?? string.Empty
			}).ToList() ?? [];
	}
}
