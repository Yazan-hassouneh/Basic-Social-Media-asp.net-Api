using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.CommentDtosValidation
{
	public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
	{
		private readonly ApplicationDbContext _context;
		public UpdateCommentDtoValidator(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;

			Include(new BaseUpdateFileValidator());
			Include(new BaseUserIdDtoValidation(userManager));

			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(CommentExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}
		private async Task<bool> CommentExists(int Id, CancellationToken cancellationToken)
		{
			return await _context.Comments.AnyAsync(comment => comment.Id == Id, cancellationToken);
		}
	}
}
