using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.CommentDtosValidation
{
	public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
	{
		private readonly ApplicationDbContext _context;
		public UpdateCommentDtoValidator(ApplicationDbContext context)
		{
			_context = context;

			Include(new BaseContentDtoValidation());

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
