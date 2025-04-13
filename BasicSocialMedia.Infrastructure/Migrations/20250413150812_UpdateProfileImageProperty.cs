using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileImageProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                schema: "Auth",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "File",
                table: "ProfileImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                schema: "File",
                table: "ProfileImages",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImages_Users_UserId",
                schema: "File",
                table: "ProfileImages",
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
                name: "FK_ProfileImages_Users_UserId",
                schema: "File",
                table: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_ProfileImages_UserId",
                schema: "File",
                table: "ProfileImages");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                schema: "Auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "File",
                table: "ProfileImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
