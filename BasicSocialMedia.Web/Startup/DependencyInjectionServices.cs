using BasicSocialMedia.Application.CustomPolicies.Block;
using BasicSocialMedia.Application.CustomPolicies.Moderation;
using BasicSocialMedia.Application.CustomPolicies.Ownership;
using BasicSocialMedia.Application.CustomPolicies.PostVisibility;
using BasicSocialMedia.Application.DTOsValidation.AuthDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.ChatDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.CommentDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.M2MDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.MessageDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.PostDtosValidation;
using BasicSocialMedia.Application.DTOsValidation.ReactionsDtosValidation;
using BasicSocialMedia.Application.Services.AuthServices;
using BasicSocialMedia.Application.Services.EnumsServices;
using BasicSocialMedia.Application.Services.FileServices;
using BasicSocialMedia.Application.Services.M2MServices;
using BasicSocialMedia.Application.Services.ModelsServices;
using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.DTOs.ChatDTOs;
using BasicSocialMedia.Core.DTOs.Comment;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.DTOs.MessageDTOs;
using BasicSocialMedia.Core.DTOs.PostDTOs;
using BasicSocialMedia.Core.DTOs.ReactionsDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EnumsServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.FileServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
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
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<IChatServices, ChatService>();
			services.AddScoped<IBlockService, BlockService>();
			services.AddScoped<IFollowService, FollowService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<IAudienceService, AudienceService>();
			services.AddScoped<IMessagesServices, MessageService>();
			services.AddScoped<IFriendshipService, FriendshipService>();
			services.AddScoped<IPostReactionService, PostReactionService>();
			services.AddScoped<ICommentReactionService, CommentReactionService>();
			return services;
		}		
		internal static IServiceCollection AddDTOsValidatorsInjection(this IServiceCollection services)
		{
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
			services.AddScoped<IValidator<AddPostReactionDto>, AddPostReactionDtoValidator>();
			services.AddScoped<IValidator<SendFollowRequestDto>, SendFollowRequestDtoValidator>();
			services.AddScoped<IValidator<SendFriendRequestDto>, SendFriendRequestDtoValidator>();
			services.AddScoped<IValidator<AddCommentReactionDto>, AddCommentReactionDtoValidator>();
			services.AddScoped<IValidator<ChangeFriendshipStatusDto>, ChangeFriendshipStatusDtoValidator>();

			return services;
		}		
		internal static IServiceCollection AddUnitOfWorkInjection(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
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
