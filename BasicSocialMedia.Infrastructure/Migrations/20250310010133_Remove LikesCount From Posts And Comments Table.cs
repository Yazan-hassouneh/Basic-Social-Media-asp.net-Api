using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLikesCountFromPostsAndCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                schema: "Posts",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                schema: "Posts",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LikesCount",
                schema: "Posts",
                table: "Posts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LikesCount",
                schema: "Posts",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
