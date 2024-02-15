using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Book_BookId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BookId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Comment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BookId",
                table: "Comment",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Book_BookId",
                table: "Comment",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id");
        }
    }
}
