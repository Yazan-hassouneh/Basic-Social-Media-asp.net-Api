using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.PostDtosValidation
{
	public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public UpdatePostDtoValidator(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Include(new BaseUpdateFileValidator());
			Include(new BaseAudienceDtoValidation());

			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationSettings.GeneralErrorMessage)
				.MustAsync(PostExists).WithMessage(ValidationSettings.GeneralErrorMessage);
		}
		private async Task<bool> PostExists(int Id, CancellationToken cancellationToken)
		{
			return await _unitOfWork.Posts.DoesExist(Id, cancellationToken);
		}
	}
}
