using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User2Id",
                schema: "Messages",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                schema: "Messages",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.AddColumn<bool>(
                name: "Current",
                schema: "File",
                table: "ProfileImages",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "DeletedMessages",
                schema: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeletedMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Messages",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedMessages_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessages_MessageId_UserId",
                schema: "Messages",
                table: "DeletedMessages",
                columns: new[] { "MessageId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeletedMessages_UserId",
                schema: "Messages",
                table: "DeletedMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedMessages",
                schema: "Messages");

            migrationBuilder.DropColumn(
                name: "Current",
                schema: "File",
                table: "ProfileImages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "Messages",
                table: "Messages",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                schema: "Messages",
                table: "Messages",
                newName: "User1Id");
        }
    }
}
