using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBackgroundJobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Blocking_BlockerId_BlockedId",
                schema: "Relations",
                table: "Blocking");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "Relations",
                table: "Friendships",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                schema: "Relations",
                table: "Friendships",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FollowingId",
                schema: "Relations",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                schema: "Relations",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BlockerId",
                schema: "Relations",
                table: "Blocking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BlockedId",
                schema: "Relations",
                table: "Blocking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "UserBackgroundJobs",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true,
                filter: "[SenderId] IS NOT NULL AND [ReceiverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Blocking_BlockerId_BlockedId",
                schema: "Relations",
                table: "Blocking",
                columns: new[] { "BlockerId", "BlockedId" },
                unique: true,
                filter: "[BlockerId] IS NOT NULL AND [BlockedId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBackgroundJobs",
                schema: "Auth");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Blocking_BlockerId_BlockedId",
                schema: "Relations",
                table: "Blocking");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "Relations",
                table: "Friendships",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                schema: "Relations",
                table: "Friendships",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FollowingId",
                schema: "Relations",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                schema: "Relations",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlockerId",
                schema: "Relations",
                table: "Blocking",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlockedId",
                schema: "Relations",
                table: "Blocking",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocking_BlockerId_BlockedId",
                schema: "Relations",
                table: "Blocking",
                columns: new[] { "BlockerId", "BlockedId" },
                unique: true);
        }
    }
}
