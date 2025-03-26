using BasicSocialMedia.Core.Consts;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Enums;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation
{
	public class ChangeFriendshipStatusDtoValidator : AbstractValidator<ChangeFriendshipStatusDto>
	{
		public ChangeFriendshipStatusDtoValidator()
		{
			RuleFor(x => x.Id).NotEmpty().NotNull();

			RuleFor(x => x.Status)
				.Must(value => Enum.IsDefined(typeof(ProjectEnums.FriendshipStatus), value))
				.WithMessage(ValidationSettings.GeneralErrorMessage);
		}
	}
}
