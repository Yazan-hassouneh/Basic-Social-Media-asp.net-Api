using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.MessageDtosValidation
{
	public class UpdateMessageDtoValidator : AbstractValidator<UpdateMessageDto>
	{
		private readonly ApplicationDbContext _context;
		public UpdateMessageDtoValidator(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;

			Include(new BaseUpdateFileValidator());
			Include(new BaseUserIdDtoValidation(userManager));

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
