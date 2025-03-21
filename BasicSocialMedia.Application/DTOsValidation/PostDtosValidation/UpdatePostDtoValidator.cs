using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Application.DTOsValidation.PostDtosValidation
{
	public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
	{
		private readonly ApplicationDbContext _context;
		public UpdatePostDtoValidator(ApplicationDbContext context)
		{
			_context = context;

			Include(new BaseContentDtoValidation());
			Include(new BaseAudienceDtoValidation());

			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(PostExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}
		private async Task<bool> PostExists(int Id, CancellationToken cancellationToken)
		{
			return await _context.Posts.AnyAsync(post => post.Id == Id, cancellationToken);
		}
	}
}
