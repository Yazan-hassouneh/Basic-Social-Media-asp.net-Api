using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notification");

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

            migrationBuilder.CreateTable(
                name: "CommentReactionNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotifiedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReactionNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReactionNotifications_UserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Notification",
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentReactionNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequestNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotifiedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequestNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequestNotifications_UserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Notification",
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequestNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewCommentNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotifiedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewCommentNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewCommentNotifications_UserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Notification",
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewCommentNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewFollowerNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotifiedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewFollowerNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewFollowerNotifications_UserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Notification",
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewFollowerNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostReactionNotifications",
                schema: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotifiedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReactionNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostReactionNotifications_UserNotifications_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Notification",
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReactionNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactionNotifications_UserId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "CommentReactionNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequestNotifications_UserId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequestNotifications_UserNotificationId",
                schema: "Notification",
                table: "FriendRequestNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewCommentNotifications_UserId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewCommentNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewCommentNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewFollowerNotifications_UserId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewFollowerNotifications_UserNotificationId",
                schema: "Notification",
                table: "NewFollowerNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReactionNotifications_UserId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReactionNotifications_UserNotificationId",
                schema: "Notification",
                table: "PostReactionNotifications",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                schema: "Notification",
                table: "UserNotifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentReactionNotifications",
                schema: "Notification");

            migrationBuilder.DropTable(
                name: "FriendRequestNotifications",
                schema: "Notification");

            migrationBuilder.DropTable(
                name: "NewCommentNotifications",
                schema: "Notification");

            migrationBuilder.DropTable(
                name: "NewFollowerNotifications",
                schema: "Notification");

            migrationBuilder.DropTable(
                name: "PostReactionNotifications",
                schema: "Notification");

            migrationBuilder.DropTable(
                name: "UserNotifications",
                schema: "Notification");
        }
    }
}
