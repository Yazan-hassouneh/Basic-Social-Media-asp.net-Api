using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.MessageDtosValidation
{
	public class UpdateMessageDtoValidator : AbstractValidator<UpdateMessageDto>
	{
		private readonly ApplicationDbContext _context;
		public UpdateMessageDtoValidator(ApplicationDbContext context)
		{
			_context = context;

			Include(new BaseUpdateFileValidator());

			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(MessageExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}
		private async Task<bool> MessageExists(int Id, CancellationToken cancellationToken)
		{
			return await _context.Messages.AnyAsync(message => message.Id == Id, cancellationToken);
		}
	}
}
