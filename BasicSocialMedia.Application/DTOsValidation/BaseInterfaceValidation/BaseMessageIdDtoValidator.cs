using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseMessageIdDtoValidator : AbstractValidator<IMessageIdDto>
	{
		private readonly ApplicationDbContext _context;

		public BaseMessageIdDtoValidator(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(x => x.MessageId)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(MessageExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}

		private async Task<bool> MessageExists(int messageId, CancellationToken cancellationToken)
		{
			return await _context.Comments.AnyAsync(message => message.Id == messageId, cancellationToken);
		}
	}
}
