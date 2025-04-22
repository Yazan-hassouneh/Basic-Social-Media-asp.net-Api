using AutoMapper;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Application.Mappers
{
	public class MessageProfile : Profile
	{
		public MessageProfile()
		{
			CreateMap<Message, GetMessagesDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User1Id, opt => opt.MapFrom(src => src.SenderId))
				.ForMember(destination => destination.User2Id, opt => opt.MapFrom(src => src.ReceiverId))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => src.Content))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => MapMessageFiles(src.Files)))
				.ForMember(destination => destination.IsRead, opt => opt.MapFrom(src => src.IsRead))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ReverseMap();
		}

		private List<GetMessageFileDto> MapMessageFiles(IEnumerable<MessageFileModel> files)
		{
			return files.Select(file => new GetMessageFileDto
			{
				Id = file.Id,
				UserId = file.UserId,
				MessageId = file.MessageId,
				ChatId = file.ChatId,
				Path = file.Path
			}).ToList();
		}
	}
}
