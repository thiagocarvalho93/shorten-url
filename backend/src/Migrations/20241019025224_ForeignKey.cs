using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShortUrlDAOId",
                table: "ClickAnalytics",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClickAnalytics_ShortUrlDAOId",
                table: "ClickAnalytics",
                column: "ShortUrlDAOId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClickAnalytics_ShortUrl_ShortUrlDAOId",
                table: "ClickAnalytics",
                column: "ShortUrlDAOId",
                principalTable: "ShortUrl",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickAnalytics_ShortUrl_ShortUrlDAOId",
                table: "ClickAnalytics");

            migrationBuilder.DropIndex(
                name: "IX_ClickAnalytics_ShortUrlDAOId",
                table: "ClickAnalytics");

            migrationBuilder.DropColumn(
                name: "ShortUrlDAOId",
                table: "ClickAnalytics");
        }
    }
}
