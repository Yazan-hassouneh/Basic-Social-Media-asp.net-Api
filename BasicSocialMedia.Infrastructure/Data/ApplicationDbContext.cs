using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Configuration.AuthConfig;
using BasicSocialMedia.Infrastructure.Configuration.FilConfig;
using BasicSocialMedia.Infrastructure.Configuration.M2MConfig;
using BasicSocialMedia.Infrastructure.Configuration.MainConfig;
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
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.AppleyIdentityTablesConfig();
			builder.AddPostsSchema();
			builder.AddMessagesSchema();
			builder.AddReactionsSchema();
			builder.AddRelationsSchema();
			builder.AddFileSchema();

			builder.ApplyConfiguration(new PostConfig());
			builder.ApplyConfiguration(new RoleConfig());
			builder.ApplyConfiguration(new ChatConfig());
			builder.ApplyConfiguration(new CommentConfig());
			builder.ApplyConfiguration(new MessageConfig());
			builder.ApplyConfiguration(new BlockingConfig());
			builder.ApplyConfiguration(new FollowingConfig());
			builder.ApplyConfiguration(new FriendshipConfig());
			builder.ApplyConfiguration(new PostReactionConfig());
			builder.ApplyConfiguration(new PostFileModelConfig());
			builder.ApplyConfiguration(new CommentReactionConfig());
			builder.ApplyConfiguration(new ApplicationUserConfig());
			builder.ApplyConfiguration(new CommentFileModelConfig());
			builder.ApplyConfiguration(new MessageFileModelConfig());
			builder.ApplyConfiguration(new ProfileImagesModelConfig());
		}
	}
}
