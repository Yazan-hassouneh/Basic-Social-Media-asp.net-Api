using BasicSocialMedia.Core.DTOs.AuthDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.EntitiesServices;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Models.AuthModels;
using Hangfire;
using Microsoft.AspNetCore.Identity;

namespace BasicSocialMedia.Application.BackgroundJobs
{
	public class AccountBackgroundJobs(UserManager<ApplicationUser> userManager, IAccountService accountService, ICommentReactionService commentReactionService, IFollowService followServices, IFriendshipService friendshipServices, IBlockService blockServices, IPostService postService, IChatServices chatServices, ICommentService commentService, IPostReactionService postReactionService)
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IPostService _postService = postService;
		private readonly IChatServices _chatServices = chatServices;
		private readonly IBlockService _blockServices = blockServices;
		private readonly IFriendshipService _friendshipServices = friendshipServices;
		private readonly IFollowService _followServices = followServices;
		private readonly ICommentService _commentService = commentService;
		private readonly IPostReactionService _postReactionService = postReactionService;
		private readonly ICommentReactionService _commentReactionService = commentReactionService;
		private readonly IAccountService _accountService = accountService;

		[AutomaticRetry(Attempts = 0)]
		public async Task<IdentityResult> HardDeleteUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException($"User with ID {userId} was not found.");

			var chats = await _chatServices.GetChatsByUserIdAsync(userId);
			if (chats != null)
			{
				foreach (var chat in chats) await _chatServices.HardDeleteChatAsync(chat.Id);
			}

			await _chatServices.SetUserIdToNull(userId);
			await _blockServices.DeleteBlockListByUserIdAsync(userId);
			await _friendshipServices.RemoveFriendsByUserIdAsync(userId);
			await _followServices.CancelAllFollowingsByFollowingIdAsync(userId);
			await _commentReactionService.DeleteCommentReactionsByUserIdAsync(userId);
			await _postReactionService.DeletePostReactionsByUserIdAsync(userId);
			await _commentService.DeleteCommentsByUserIdAsync(userId);

			bool success = await _postService.DeleteAllPostsByUserIdAsync(user.Id);

			if (success)
			{
				var result = await _userManager.DeleteAsync(user);
				if (!result.Succeeded)
				{
					// Optionally log or throw depending on your style  
					throw new InvalidOperationException($"Failed to delete user with ID {userId}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
				}

				return result;
			}

			return IdentityResult.Failed(new IdentityError { Description = "Failed to delete all posts for the user." });
		}
		public async Task<bool> CancelUserHardDeletionAsync(LoginAccountDto loginInfo)
		{
			return  await _accountService.TryCancelScheduledDeletionAsync(loginInfo);
		}
	}
}
