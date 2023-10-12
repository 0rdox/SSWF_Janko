using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class testMigration123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2023, 10, 9, 11, 50, 51, 984, DateTimeKind.Local).AddTicks(8245));

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 9, 11, 50, 51, 984, DateTimeKind.Local).AddTicks(8283));
        }
    }
}
