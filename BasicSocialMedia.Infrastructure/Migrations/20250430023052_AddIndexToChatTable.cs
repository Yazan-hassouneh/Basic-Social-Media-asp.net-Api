using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                unique: true,
                filter: "[User1Id] IS NOT NULL AND [User2Id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
