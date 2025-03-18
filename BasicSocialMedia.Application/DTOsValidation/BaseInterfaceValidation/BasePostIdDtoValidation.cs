using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation
{
	internal class BasePostIdDtoValidation : AbstractValidator<IPostIdDto>
	{
		private readonly ApplicationDbContext _context;

		public BasePostIdDtoValidation(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(x => x.PostId)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(PostExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}

		private async Task<bool> PostExists(int postId, CancellationToken cancellationToken)
		{
			return await _context.Posts.AnyAsync(p => p.Id == postId, cancellationToken);
		}
	}
}
