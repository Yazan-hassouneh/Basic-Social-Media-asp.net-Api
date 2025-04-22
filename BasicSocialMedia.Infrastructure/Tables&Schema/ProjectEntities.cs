using BasicSocialMedia.Core.Models.FileModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Core.Models.Messaging;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Tables_Schema
{
	internal static class ProjectEntities
	{
		public static void AddPostsSchema(this ModelBuilder builder)
		{
			builder.Entity<Post>().ToTable("Posts", schema : "Posts");
			builder.Entity<Comment>().ToTable("Comments", schema : "Posts");
		}		
		public static void AddReactionsSchema(this ModelBuilder builder)
		{
			builder.Entity<PostReaction>().ToTable("PostReactions", schema : "Reactions");
			builder.Entity<CommentReaction>().ToTable("CommentReactions", schema : "Reactions");
		}		
		public static void AddMessagesSchema(this ModelBuilder builder)
		{
			builder.Entity<Chat>().ToTable("Chats", schema : "Messages");
			builder.Entity<Message>().ToTable("Messages", schema : "Messages");
			builder.Entity<DeletedMessage>().ToTable("DeletedMessages", schema : "Messages");
			builder.Entity<ChatDeletion>().ToTable("ChatDeletions", schema : "Messages");
		}
		public static void AddRelationsSchema(this ModelBuilder builder)
		{
			builder.Entity<Follow>().ToTable("Follows", schema : "Relations");
			builder.Entity<Friendship>().ToTable("Friendships", schema : "Relations");
			builder.Entity<Block>().ToTable("Blocking", schema : "Relations");
		}		
		public static void AddFileSchema(this ModelBuilder builder)
		{
			builder.Entity<PostFileModel>().ToTable("PostFiles", schema : "File");
			builder.Entity<CommentFileModel>().ToTable("CommentFiles", schema : "File");
			builder.Entity<MessageFileModel>().ToTable("MessageFiles", schema : "File");
			builder.Entity<ProfileImageModel>().ToTable("ProfileImages", schema : "File");
		}
	}
}
