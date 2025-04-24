using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeletedMessages_Users_UserId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Messages",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatDeletions",
                schema: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatDeletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatDeletions_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "Messages",
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessages_ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatDeletions_ChatId_UserId",
                schema: "Messages",
                table: "ChatDeletions",
                columns: new[] { "ChatId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedMessages_Chats_ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "ChatId",
                principalSchema: "Messages",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedMessages_Users_UserId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeletedMessages_Chats_ChatId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DeletedMessages_Users_UserId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.DropTable(
                name: "ChatDeletions",
                schema: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_DeletedMessages_ChatId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedMessages_Users_UserId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
