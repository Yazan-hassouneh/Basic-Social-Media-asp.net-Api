﻿using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.PostFileModelsDTOs
{
	public class UpdatePostFileDtoValidator : AbstractValidator<UpdatePostFileDto>
	{
		public UpdatePostFileDtoValidator(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			Include(new BaseUserIdDtoValidation(userManager));
			Include(new BasePostIdDtoValidation(context));
			Include(new BaseIFormFileAndMediaPathValidator());
		}
	}
}
