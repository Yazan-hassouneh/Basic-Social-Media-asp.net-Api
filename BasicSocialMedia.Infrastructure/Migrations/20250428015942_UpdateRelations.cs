using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                schema: "Reactions",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                schema: "Reactions",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "Posts",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "Posts",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                schema: "Reactions",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Users_UserId",
                schema: "Reactions",
                table: "PostReactions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                schema: "Reactions",
                table: "CommentReactions",
                column: "CommentId",
                principalSchema: "Posts",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                schema: "Reactions",
                table: "CommentReactions",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "Posts",
                table: "Comments",
                column: "PostId",
                principalSchema: "Posts",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "Posts",
                table: "Comments",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                schema: "Reactions",
                table: "PostReactions",
                column: "PostId",
                principalSchema: "Posts",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Users_UserId",
                schema: "Reactions",
                table: "PostReactions",
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
                name: "FK_CommentReactions_Comments_CommentId",
                schema: "Reactions",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                schema: "Reactions",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "Posts",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "Posts",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                schema: "Reactions",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Users_UserId",
                schema: "Reactions",
                table: "PostReactions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                schema: "Reactions",
                table: "CommentReactions",
                column: "CommentId",
                principalSchema: "Posts",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                schema: "Reactions",
                table: "CommentReactions",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "Posts",
                table: "Comments",
                column: "PostId",
                principalSchema: "Posts",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "Posts",
                table: "Comments",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                schema: "Reactions",
                table: "PostReactions",
                column: "PostId",
                principalSchema: "Posts",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Users_UserId",
                schema: "Reactions",
                table: "PostReactions",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
