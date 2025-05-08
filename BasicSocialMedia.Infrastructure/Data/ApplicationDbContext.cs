using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Core.Models.Notification;
using BasicSocialMedia.Infrastructure.Configuration.AuthConfig;
using BasicSocialMedia.Infrastructure.Configuration.FilConfig;
using BasicSocialMedia.Infrastructure.Configuration.M2MConfig;
using BasicSocialMedia.Infrastructure.Configuration.MainConfig;
using BasicSocialMedia.Infrastructure.Configuration.MessagingConfig;
using BasicSocialMedia.Infrastructure.Configuration.NotificationConfig;
using BasicSocialMedia.Infrastructure.Tables_Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		public DbSet<Post> Posts { get; set; }
		public DbSet<PostFileModel> PostFiles { get; set; }
		public DbSet<CommentFileModel> CommentFiles { get; set; }
		public DbSet<MessageFileModel> MessageFiles { get; set; }
		public DbSet<ProfileImageModel> ProfileImages { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Follow> Follows { get; set; }
		public DbSet<Friendship> Friendships { get; set; }
		public DbSet<Block> Blocking { get; set; }
		public DbSet<CommentReaction> CommentReactions { get; set; }
		public DbSet<PostReaction> PostReactions { get; set; }
		public DbSet<DeletedMessage> DeletedMessages { get; set; }
		public DbSet<ChatDeletion> ChatDeletions { get; set; }
		public DbSet<UserBackgroundJob> UserBackgroundJobs { get; set; }
		public DbSet<NewCommentNotification> NewCommentNotifications { get; set; }
		public DbSet<CommentReactionNotification> CommentReactionNotifications { get; set; }
		public DbSet<NewFollowerNotification> NewFollowerNotifications { get; set; }
		public DbSet<FriendRequestNotification> FriendRequestNotifications { get; set; }
		public DbSet<PostReactionNotification> PostReactionNotifications{ get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.AppleyIdentityTablesConfig();
			builder.AddFileSchema();
			builder.AddPostsSchema();
			builder.AddMessagesSchema();
			builder.AddReactionsSchema();
			builder.AddRelationsSchema();
			builder.AddNotificationSchema();

			builder.ApplyConfiguration(new PostConfig());
			builder.ApplyConfiguration(new RoleConfig());
			builder.ApplyConfiguration(new ChatConfig());
			builder.ApplyConfiguration(new CommentConfig());
			builder.ApplyConfiguration(new MessageConfig());
			builder.ApplyConfiguration(new BlockingConfig());
			builder.ApplyConfiguration(new FollowingConfig());
			builder.ApplyConfiguration(new FriendshipConfig());
			builder.ApplyConfiguration(new ChatDeletionConfig());
			builder.ApplyConfiguration(new PostReactionConfig());
			builder.ApplyConfiguration(new PostFileModelConfig());
			builder.ApplyConfiguration(new DeletedMessageConfig());
			builder.ApplyConfiguration(new CommentReactionConfig());
			builder.ApplyConfiguration(new ApplicationUserConfig());
			builder.ApplyConfiguration(new CommentFileModelConfig());
			builder.ApplyConfiguration(new MessageFileModelConfig());
			builder.ApplyConfiguration(new UserBackgroundJobConfig());
			builder.ApplyConfiguration(new ProfileImagesModelConfig());
			builder.ApplyConfiguration(new NewCommentNotificationConfig());
			builder.ApplyConfiguration(new NewFollowerNotificationConfig());
			builder.ApplyConfiguration(new PostReactionNotificationConfig());
			builder.ApplyConfiguration(new FriendRequestNotificationConfig());
			builder.ApplyConfiguration(new CommentReactionNotificationConfig());
		}
	}
}
