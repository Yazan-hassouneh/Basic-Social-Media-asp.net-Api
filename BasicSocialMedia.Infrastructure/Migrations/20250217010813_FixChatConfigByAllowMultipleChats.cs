﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChatConfigByAllowMultipleChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chats_User1Id_User2Id",
                schema: "Messages",
                table: "Chats");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User1Id",
                schema: "Messages",
                table: "Chats",
                column: "User1Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chats_User1Id",
                schema: "Messages",
                table: "Chats");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User1Id_User2Id",
                schema: "Messages",
                table: "Chats",
                columns: new[] { "User1Id", "User2Id" },
                unique: true);
        }
    }
}
