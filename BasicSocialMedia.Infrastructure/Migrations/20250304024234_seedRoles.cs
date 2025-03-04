using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
					table: "Roles",
					schema: "Auth",
					columns: ["Id", "Name", "NormalizedName", "ConcurrencyStamp"],
					values:
					[
						Guid.NewGuid().ToString(),
						"User",
						"USER",
						Guid.NewGuid().ToString(),
					]
				);
			migrationBuilder.InsertData(
					table: "Roles",
					schema: "Auth",
					columns: ["Id", "Name", "NormalizedName", "ConcurrencyStamp"],
					values:
					[
						Guid.NewGuid().ToString(),
						"Admin",
						"ADMIN",
						Guid.NewGuid().ToString(),
					]
				);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DELETE FROM [Roles]");

		}
	}
}
