﻿using BasicSocialMedia.Application.BackgroundJobs;
using BasicSocialMedia.Application.CustomPolicies.Block;
using BasicSocialMedia.Application.CustomPolicies.Moderation;
using BasicSocialMedia.Application.CustomPolicies.Ownership;
using BasicSocialMedia.Application.CustomPolicies.PostVisibility;
using BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.BaseInterfaceValidation.File;
using BasicSocialMedia.Application.DTOsValidation.ChatDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.CommentDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.CommentFileModesDTOs;
using BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.MessageFileModelsDTOs;
using BasicSocialMedia.Application.DTOsValidation.FileModelsDTOs.PostFileModelsDTOs;
using BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.MessageDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.PostDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.ProfileImage;
using BasicSocialMedia.Application.DTOsValidation.ReactionsDtosValidation;
using BasicSocialMedia.Application.Services.AuthServices;
using BasicSocialMedia.Application.Services.EnumsServices;
using BasicSocialMedia.Application.Services.FileModelServices;
using BasicSocialMedia.Application.Services.FileServices;
using BasicSocialMedia.Application.Services.M2MServices;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Application.Services.NotificationServices;
using BasicSocialMedia.Application.Services.ValidationServices;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.FileModelsDTOs;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.DTOs.ProfileImage;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileModelsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.NotificationsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.ValidationServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Interfaces.UnitsOfWork;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using BasicSocialMedia.Infrastructure.UnitsOfWork;
using FluentValidation;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;

namespace BasicSocialMedia.Web.Startup
{
	public static class DependencyInjectionServices
	{
		internal static IServiceCollection AddServicesInjection(this IServiceCollection services)
		{
			services.AddScoped<IJWTService, JWTService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<IChatServices, ChatService>();
			services.AddScoped<IBlockService, BlockService>();
			services.AddScoped<IFollowService, FollowService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<IAudienceService, AudienceService>();
			services.AddScoped<IMessagesServices, MessageService>();
			services.AddScoped<IFriendshipService, FriendshipService>();
			services.AddScoped<IPostReactionService, PostReactionService>();
			services.AddScoped<IPostFileModelService, PostFileModelService>();
			services.AddScoped<IFileValidationResult, FileValidationResult>();
			services.AddScoped<ICommentReactionService, CommentReactionService>();
			services.AddScoped<ICommentFileModeService, CommentFileModelService>();
			services.AddScoped<IMessageFileModelService, MessageFileModelService>();
			services.AddScoped<IProfileImageModelService, ProfileImageModelService>();
			services.AddScoped<IUserBackgroundJobsServices, UserBackgroundJobsServices>();
			services.AddScoped(typeof(IBaseNotificationService<>), typeof(BaseNotificationService<>));
			services.AddScoped<IBaseNotificationService<NewCommentNotification>, BaseNotificationService<NewCommentNotification>>();
			services.AddScoped<IBaseNotificationService<NewFollowerNotification>, BaseNotificationService<NewFollowerNotification>>();
			services.AddScoped<IBaseNotificationService<PostReactionNotification>, BaseNotificationService<PostReactionNotification>>();
			services.AddScoped<IBaseNotificationService<FriendRequestNotification>, BaseNotificationService<FriendRequestNotification>>();
			services.AddScoped<IBaseNotificationService<CommentReactionNotification>, BaseNotificationService<CommentReactionNotification>>();
			// Repeat for other types

			return services;
		}		
		internal static IServiceCollection AddDTOsValidatorsInjection(this IServiceCollection services)
		{
			services.AddScoped<IValidator<IFormFile>, BaseSingleFileValidator>();
			services.AddScoped<IValidator<AddRoleDto>, AddRoleDtoValidator>();
			services.AddScoped<IValidator<AddChatDto>, AddChatDtoValidator>();
			services.AddScoped<IValidator<AddPostDto>, AddPostDtoValidator>();
			services.AddScoped<IValidator<BlockUserDto>, BlockUserDtoValidator>();
			services.AddScoped<IValidator<AddToRoleDto>, AddToRoleDtoValidator>();
			services.AddScoped<IValidator<AddCommentDto>, AddCommentDtoValidator>();
			services.AddScoped<IValidator<AddMessageDto>, AddMessageDtoValidator>();
			services.AddScoped<IValidator<UpdatePostDto>, UpdatePostDtoValidator>();
			services.AddScoped<IValidator<LoginAccountDto>, LoginAccountDtoValidator>();
			services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
			services.AddScoped<IValidator<UpdateCommentDto>, UpdateCommentDtoValidator>();
			services.AddScoped<IValidator<UpdateMessageDto>, UpdateMessageDtoValidator>();
			services.AddScoped<IValidator<AddProfileImageDto>, AddProfileImageValidator>();
			services.AddScoped<IValidator<AddPostFileDto>, AddPostFileModelDtoValidator>();
			services.AddScoped<IValidator<AddCommentFileDto>, AddCommentFileDtoValidator>();
			services.AddScoped<IValidator<AddMessageFileDto>, AddMessageFileDtoValidator>();
			services.AddScoped<IValidator<UpdatePostFileDto>, UpdatePostFileDtoValidator>();
			services.AddScoped<IValidator<AddPostReactionDto>, AddPostReactionDtoValidator>();
			services.AddScoped<IValidator<UpdateCommentFileDto>, UpdateCommentFileDtoValidator>();
			services.AddScoped<IValidator<SendFollowRequestDto>, SendFollowRequestDtoValidator>();
			services.AddScoped<IValidator<UpdateMessageFileDto>, UpdateMessageFileDtoValidator>();
			services.AddScoped<IValidator<SendFriendRequestDto>, SendFriendRequestDtoValidator>();
			services.AddScoped<IValidator<AddCommentReactionDto>, AddCommentReactionDtoValidator>();
			services.AddScoped<IValidator<ChangeFriendshipStatusDto>, ChangeFriendshipStatusDtoValidator>();

			return services;
		}		
		internal static IServiceCollection AddUnitOfWorkInjection(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<INotificationUnitOfWork, NotificationUnitOfWork>();
			return services;
		}		
		internal static IServiceCollection AddRepositoryInjection(this IServiceCollection services)
		{
			services.AddScoped(typeof(IBaseNotificationRepository<>), typeof(BaseNotificationRepository<>));
			return services;
		}		
		internal static IServiceCollection AddBackgroundJobsInjection(this IServiceCollection services)
		{
			
			return services;
		}			
		internal static IServiceCollection AddCustomPoliciesInjection(this IServiceCollection services)
		{
			services.AddScoped<IAuthorizationHandler, PostVisibilityHandler>();
			services.AddScoped<IAuthorizationHandler, BlockedHandler>();
			services.AddSingleton<IAuthorizationHandler, PostModerationHandler>();
			services.AddSingleton<IAuthorizationHandler, OwnershipHandler>();
			return services;
		}		
		internal static IServiceCollection AddHtmlSanitizerInjection(this IServiceCollection services)
		{
			services.AddSingleton(provider =>
			{
				var sanitizer = new HtmlSanitizer();

				// Configure allowed HTML tags
				sanitizer.AllowedTags.Clear(); // remove All Allowed Tags
				// Note : When it remove tag, it remove it's content with it.

				sanitizer.AllowedTags.Add("p"); // Allow paragraph
				sanitizer.AllowedTags.Add("b"); // Allow bold
				sanitizer.AllowedTags.Add("i"); // Allow italic
				

				// Configure allowed attributes
				//sanitizer.AllowedAttributes.Clear();
				//sanitizer.AllowedAttributes.Add("href"); // Allow href on <a>
				//sanitizer.AllowedAttributes.Add("class"); // Allow CSS classes

				//// Configure allowed CSS properties
				//sanitizer.AllowedCssProperties.Add("color"); // Allow color styling

				return sanitizer;
			});
			return services;
		}		

	}
}
