using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId1",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId2",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_UserId1_UserId2",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_UserId2",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                schema: "Relations",
                table: "Friendships",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                schema: "Relations",
                table: "Friendships",
                newName: "ReceiverId");

            migrationBuilder.CreateTable(
                name: "Blocking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    BlockerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlockedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocking_Users_BlockedId",
                        column: x => x.BlockedId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blocking_Users_BlockerId",
                        column: x => x.BlockerId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ReceiverId",
                schema: "Relations",
                table: "Friendships",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocking_BlockedId",
                table: "Blocking",
                column: "BlockedId");

            migrationBuilder.CreateIndex(
                name: "IX_Blocking_BlockerId_BlockedId",
                table: "Blocking",
                columns: new[] { "BlockerId", "BlockedId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_ReceiverId",
                schema: "Relations",
                table: "Friendships",
                column: "ReceiverId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_SenderId",
                schema: "Relations",
                table: "Friendships",
                column: "SenderId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_ReceiverId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_SenderId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropTable(
                name: "Blocking");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_ReceiverId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                schema: "Relations",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "Relations",
                table: "Friendships",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                schema: "Relations",
                table: "Friendships",
                newName: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId1_UserId2",
                schema: "Relations",
                table: "Friendships",
                columns: new[] { "UserId1", "UserId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId2",
                schema: "Relations",
                table: "Friendships",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId1",
                schema: "Relations",
                table: "Friendships",
                column: "UserId1",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId2",
                schema: "Relations",
                table: "Friendships",
                column: "UserId2",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
