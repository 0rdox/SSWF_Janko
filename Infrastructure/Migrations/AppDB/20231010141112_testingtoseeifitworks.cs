using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class testingtoseeifitworks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserClaim<string>");

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2023, 10, 10, 16, 11, 12, 403, DateTimeKind.Local).AddTicks(8602));

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 10, 16, 11, 12, 403, DateTimeKind.Local).AddTicks(8662));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityUserClaim<string>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim<string>", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityUserClaim<string>",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Student", "true", "1" },
                    { 2, "Student", "true", "2" },
                    { 3, "Student", "true", "3" }
                });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2023, 10, 10, 16, 0, 50, 204, DateTimeKind.Local).AddTicks(851));

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 10, 16, 0, 50, 204, DateTimeKind.Local).AddTicks(903));
        }
    }
}
