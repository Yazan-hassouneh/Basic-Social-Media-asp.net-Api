using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.CommentDtosValidation
{
	public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
	{
		public AddCommentDtoValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			Include(new BaseContentDtoValidation());
			Include(new BasePostIdDtoValidation(context));
			Include(new BaseUserIdDtoValidation(userManager));
		}
	}
}
