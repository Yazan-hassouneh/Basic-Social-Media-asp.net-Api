using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.Messaging;

namespace BasicSocialMedia.Application.Mappers
{
	public class ChatProfile : BaseProfile
	{
		public ChatProfile() 
		{
			CreateMap<Chat, GetChatDto>()
				.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(destination => destination.User1Id, opt => opt.MapFrom(src => src.User1Id))
				.ForMember(destination => destination.User2Id, opt => opt.MapFrom(src => src.User2Id))
				.ForMember(destination => destination.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
				.ForMember(destination => destination.User1, opt => opt.MapFrom(src => MapUser(src.User1)))
				.ForMember(destination => destination.User2, opt => opt.MapFrom(src => MapUser(src.User2)))
				.ForMember(destination => destination.Files, opt => opt.MapFrom(src => MapChatFiles(src.Files)))
				.ForMember(destination => destination.Messages, opt => opt.MapFrom(src => MapChatMessages(src.Messages)))
				.ReverseMap();
		}

		private static List<GetMessagesDto> MapChatMessages(IEnumerable<Message> messages)
		{
			return messages.Select(message => new GetMessagesDto
			{
				Id = message.Id,
				SenderId = message.SenderId,
				ReceiverId = message.ReceiverId,
				Content = message.Content,
				CreatedOn = message.CreatedOn,
				IsRead = message.IsRead,
				Files = MapChatFiles(message.Files)
			}).ToList();
		}
		private static List<GetMessageFileDto> MapChatFiles(IEnumerable<MessageFileModel> files)
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
