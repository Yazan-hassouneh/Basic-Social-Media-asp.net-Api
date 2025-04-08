using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                schema: "Relations",
                table: "Follows");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Posts",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MediaPath",
                schema: "Posts",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Messages",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MediaPath",
                schema: "Messages",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Posts",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MediaPath",
                schema: "Posts",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaPath",
                schema: "Posts",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "MediaPath",
                schema: "Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MediaPath",
                schema: "Posts",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Posts",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Messages",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                schema: "Relations",
                table: "Follows",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "Posts",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
