using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "ShortUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false),
                    long_url = table.Column<string>(type: "VARCHAR", maxLength: 280, nullable: false),
                    created_at = table.Column<DateTime>(type: "DATE", nullable: false),
                    expires_at = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrl", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_URL_TOKEN",
                table: "ShortUrl",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrl");
        }
    }
}
