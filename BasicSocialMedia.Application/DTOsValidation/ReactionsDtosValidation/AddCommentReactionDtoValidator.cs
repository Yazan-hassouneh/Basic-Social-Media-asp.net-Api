using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.ReactionsDtosValidation
{
	public class AddCommentReactionDtoValidator : AbstractValidator<AddCommentReactionDto>
	{
		public AddCommentReactionDtoValidator(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			Include(new BaseCommentIdDtoValidation(context));
			Include(new BaseUserIdDtoValidation(userManager));
		}
	}
}
