using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseChatIdDtoValidator : AbstractValidator<IChatIdDto>
	{
		private readonly ApplicationDbContext _context;

		public BaseChatIdDtoValidator(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(x => x.ChatId)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(ChatExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}

		private async Task<bool> ChatExists(int chatId, CancellationToken cancellationToken)
		{
			return await _context.Chats.AnyAsync(chat => chat.Id == chatId, cancellationToken);
		}
	}
}
