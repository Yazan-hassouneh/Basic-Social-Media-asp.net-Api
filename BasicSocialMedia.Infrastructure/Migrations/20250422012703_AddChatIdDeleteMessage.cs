using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatIdDeleteMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessages_ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeletedMessages_Chats_ChatId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "ChatId",
                principalSchema: "Messages",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeletedMessages_Chats_ChatId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.DropIndex(
                name: "IX_DeletedMessages_ChatId",
                schema: "Messages",
                table: "DeletedMessages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                schema: "Messages",
                table: "DeletedMessages");
        }
    }
}
