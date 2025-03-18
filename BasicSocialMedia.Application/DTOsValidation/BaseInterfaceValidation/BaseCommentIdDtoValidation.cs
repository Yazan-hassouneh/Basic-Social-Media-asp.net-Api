using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BaseCommentIdDtoValidation : AbstractValidator<ICommentIdDto>
	{
		private readonly ApplicationDbContext _context;

		public BaseCommentIdDtoValidation(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(x => x.CommentId)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(CommentExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}

		private async Task<bool> CommentExists(int CommentId, CancellationToken cancellationToken)
		{
			return await _context.Comments.AnyAsync(comment => comment.Id == CommentId, cancellationToken);
		}
	}
}
