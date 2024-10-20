using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyLinkUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Link",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Link_UserId",
                table: "Link",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Users_UserId",
                table: "Link",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Link_Users_UserId",
                table: "Link");

            migrationBuilder.DropIndex(
                name: "IX_Link_UserId",
                table: "Link");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Link");
        }
    }
}
