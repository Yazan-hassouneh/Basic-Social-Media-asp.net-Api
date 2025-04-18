﻿using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using FluentValidation;

namespace BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation
{
	public class AddRoleDtoValidator : AbstractValidator<AddRoleDto>
	{
		public AddRoleDtoValidator()
		{
			Include(new BaseRoleNameDtoValidator());
		}
	}
}
