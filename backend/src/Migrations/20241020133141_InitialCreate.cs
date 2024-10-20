using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortCode = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false),
                    OriginalUrl = table.Column<string>(type: "VARCHAR", maxLength: 280, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATE", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Click",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LinkId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClickDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    IpAdress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Referrer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Click", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Click_Link_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Link",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Click_LinkId",
                table: "Click",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_URL_TOKEN",
                table: "Link",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Click");

            migrationBuilder.DropTable(
                name: "Link");
        }
    }
}
