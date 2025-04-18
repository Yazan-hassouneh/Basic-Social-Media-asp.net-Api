﻿using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Models.MainModels;

namespace BasicSocialMedia.Core.Interfaces.Repos
{
	public interface IMessageRepository : IBaseRepository<Message>
	{
		Task<string?> GetUserId(int messageId);
		Task<IEnumerable<Message?>> GetAllAsync(int chatId);
	}
}
