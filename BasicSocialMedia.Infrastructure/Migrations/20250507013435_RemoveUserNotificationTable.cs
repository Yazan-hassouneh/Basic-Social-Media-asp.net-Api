using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactionNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "CommentReactionNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequestNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequestNotifications_Users_UserId",
                schema: "Notification",
                table: "FriendRequestNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewCommentNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewCommentNotifications_Users_UserId",
                schema: "Notification",
                table: "NewCommentNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewFollowerNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewFollowerNotifications_Users_UserId",
                schema: "Notification",
                table: "NewFollowerNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactionNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "PostReactionNotifications");

            migrationBuilder.DropTable(
                name: "UserNotifications",
                schema: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_PostReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications");

            migrationBuilder.DropIndex(
                name: "IX_NewFollowerNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications");

            migrationBuilder.DropIndex(
                name: "IX_NewCommentNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequestNotifications_UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommentReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications");

            migrationBuilder.DropColumn(
                name: "UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications");

            migrationBuilder.DropColumn(
                name: "UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications");

            migrationBuilder.DropColumn(
                name: "UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications");

            migrationBuilder.DropColumn(
                name: "UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications");

            migrationBuilder.DropColumn(
                name: "UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequestNotifications_Users_UserId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewCommentNotifications_Users_UserId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewFollowerNotifications_Users_UserId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "CommentReactionNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequestNotifications_Users_UserId",
                schema: "Notification",
                table: "FriendRequestNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewCommentNotifications_Users_UserId",
                schema: "Notification",
                table: "NewCommentNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NewFollowerNotifications_Users_UserId",
                schema: "Notification",
                table: "NewFollowerNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "PostReactionNotifications");

            migrationBuilder.AddColumn<int>(
                name: "UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewFollowerNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewCommentNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequestNotifications_UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                schema: "Notification",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactionNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserNotificationId",
                principalSchema: "Notification",
                principalTable: "UserNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequestNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserNotificationId",
                principalSchema: "Notification",
                principalTable: "UserNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequestNotifications_Users_UserId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewCommentNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserNotificationId",
                principalSchema: "Notification",
                principalTable: "UserNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewCommentNotifications_Users_UserId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewFollowerNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserNotificationId",
                principalSchema: "Notification",
                principalTable: "UserNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewFollowerNotifications_Users_UserId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactionNotifications_UserNotifications_UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserNotificationId",
                principalSchema: "Notification",
                principalTable: "UserNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactionNotifications_Users_UserId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
