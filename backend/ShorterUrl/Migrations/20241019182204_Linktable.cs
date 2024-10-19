using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShorterUrl.Migrations
{
    /// <inheritdoc />
    public partial class Linktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickAnalytics");

            migrationBuilder.DropTable(
                name: "ShortUrl");

            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    short_code = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false),
                    original_url = table.Column<string>(type: "VARCHAR", maxLength: 280, nullable: false),
                    created_at = table.Column<DateTime>(type: "DATE", nullable: false),
                    expires_at = table.Column<DateTime>(type: "DATE", nullable: false)
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
                    Referrer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    LinkModelId = table.Column<int>(type: "INTEGER", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Click_Link_LinkModelId",
                        column: x => x.LinkModelId,
                        principalTable: "Link",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Click_LinkId",
                table: "Click",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Click_LinkModelId",
                table: "Click",
                column: "LinkModelId");

            migrationBuilder.CreateIndex(
                name: "IX_URL_TOKEN",
                table: "Link",
                column: "short_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Click");

            migrationBuilder.DropTable(
                name: "Link");

            migrationBuilder.CreateTable(
                name: "ShortUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_at = table.Column<DateTime>(type: "DATE", nullable: false),
                    expires_at = table.Column<DateTime>(type: "DATE", nullable: false),
                    original_url = table.Column<string>(type: "VARCHAR", maxLength: 280, nullable: false),
                    short_code = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClickAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortUrlId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClickDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    IpAdress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Referrer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ShortUrlDAOId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickAnalytics_ShortUrl_ShortUrlDAOId",
                        column: x => x.ShortUrlDAOId,
                        principalTable: "ShortUrl",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClickAnalytics_ShortUrl_ShortUrlId",
                        column: x => x.ShortUrlId,
                        principalTable: "ShortUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickAnalytics_ShortUrlDAOId",
                table: "ClickAnalytics",
                column: "ShortUrlDAOId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickAnalytics_ShortUrlId",
                table: "ClickAnalytics",
                column: "ShortUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_URL_TOKEN",
                table: "ShortUrl",
                column: "short_code",
                unique: true);
        }
    }
}
