using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.MessageDtosValidation
{
	public class AddMessageDtoValidator : AbstractValidator<AddMessageDto>
	{
		public AddMessageDtoValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseContentDtoValidation());
			Include(new BaseSenderIdReceiverIdDtoValidation(userManager));
		}
	}
}
