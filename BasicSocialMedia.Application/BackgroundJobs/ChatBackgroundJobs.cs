using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using Hangfire;

namespace BasicSocialMedia.Application.BackgroundJobs
{
	public class ChatBackgroundJobs( IChatServices chatServices)
	{
		private readonly IChatServices _chatServices = chatServices;

		[AutomaticRetry(Attempts = 0)]
		public async Task<bool> HardDeleteChat(int chatId)
		{
			await _chatServices.HardDeleteChatAsync(chatId);
			return true;
		}
	}
}
