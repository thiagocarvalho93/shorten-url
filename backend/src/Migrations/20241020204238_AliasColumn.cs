using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class AliasColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Link",
                type: "VARCHAR",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Link");
        }
    }
}
