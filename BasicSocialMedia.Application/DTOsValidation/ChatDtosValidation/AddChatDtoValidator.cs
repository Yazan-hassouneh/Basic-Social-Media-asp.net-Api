using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.ChatDtosValidation
{
	public class AddChatDtoValidator : AbstractValidator<AddChatDto>
	{
		public AddChatDtoValidator(UserManager<ApplicationUser> userManager)
		{
			Include(new BaseSenderIdReceiverIdDtoValidation(userManager));
		}
	}
}
