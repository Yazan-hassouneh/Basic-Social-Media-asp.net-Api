using AutoMapper;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.Messaging;
using Ganss.Xss;
using Microsoft.Security.Application;
using System.Net;

namespace BasicSocialMedia.Application.Mappers
{
	public class MessageProfile : Profile
	{
		private readonly HtmlSanitizer _sanitizer;
		public MessageProfile(HtmlSanitizer sanitizer)
		{
			_sanitizer = sanitizer;

			CreateMap<Message, GetMessagesDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.SenderId, opt => opt.MapFrom(src => src.SenderId))
				.ForMember(destination => destination.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => WebUtility.HtmlEncode(src.Content)))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => MapFromMessageFileToGEtMessageFiles(src.Files)))
				.ForMember(destination => destination.IsRead, opt => opt.MapFrom(src => src.IsRead))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ReverseMap();

			CreateMap<AddMessageDto, AddMessageFileDto>()
				.ForMember(destination => destination.ChatId, opt => opt.MapFrom(src => src.ChatId))
				.ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.ReceiverId))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => src.Files));

			CreateMap<AddMessageDto, Message>()
				.ForMember(destination => destination.ChatId, opt => opt.MapFrom(src => src.ChatId))
				.ForMember(destination => destination.SenderId, opt => opt.MapFrom(src => src.ReceiverId))
				.ForMember(destination => destination.ReceiverId, opt => opt.MapFrom(src => src.SenderId))
				.ForMember(destination => destination.Content, opt => opt.MapFrom(src => SanitizeContent(src.Content)))
				.ForMember(destination => destination.Files, opt => opt.Ignore());

			CreateMap<Message, DeletedMessage>()
				.ForMember(destination => destination.ChatId, opt => opt.MapFrom(src => src.ChatId))
				.ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.SenderId))
				.ForMember(destination => destination.MessageId, opt => opt.MapFrom(src => src.Id));

			CreateMap<Message, MessageExistDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.SenderId, opt => opt.MapFrom(src => src.SenderId))
				.ForMember(destination => destination.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId));

			CreateMap<UpdateMessageDto, UpdateMessageFileDto>()
				.ForMember(destination => destination.MessageId, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(destination => destination.MediaPaths, opt => opt.MapFrom(src => src.MediaPaths))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => src.Files));
		}

		private static List<GetMessageFileDto> MapFromMessageFileToGEtMessageFiles(IEnumerable<MessageFileModel> files)
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

		private string SanitizeContent(string? content)
		{
			return _sanitizer.Sanitize(content ?? string.Empty);
		}
	}
}
