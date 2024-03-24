using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Novle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Admin", "ADMIN" },
                    { 2, null, "Editor", "EDITOR" },
                    { 3, null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "Birthday", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Introduction", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Settings", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, null, null, "c470f6a2-bfd1-4bbe-90ca-47eae23e0e13", "phucdk311@gmail.com", true, "Phuc DK", null, false, null, "PHUCDK311@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEAr4V77Ilp99xOXyfMWXwnxXOlUTdiF1bIIEJ3wIbZyjzbNk/1a98TV2phR8WgGAvA==", null, false, "e151285a-5994-4a75-9346-81a59e68dd8f", null, false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
