using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class ColumnDeviceLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceLanguage",
                table: "Click",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceLanguage",
                table: "Click");
        }
    }
}
