using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Application.DTOsValidation.ReactionsDtosValidation
{
	public class AddPostReactionDtoValidator : AbstractValidator<AddPostReactionDto>
	{
		public AddPostReactionDtoValidator(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			Include(new BasePostIdDtoValidation(context));
			Include(new BaseUserIdDtoValidation(userManager));
		}
	}
}
