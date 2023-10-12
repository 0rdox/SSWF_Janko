using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class SeedMigrationTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacketProduct_Packets_packetsId",
                table: "PacketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PacketProduct",
                table: "PacketProduct");

            migrationBuilder.DropIndex(
                name: "IX_PacketProduct_packetsId",
                table: "PacketProduct");

            migrationBuilder.RenameColumn(
                name: "packetsId",
                table: "PacketProduct",
                newName: "PacketsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PacketProduct",
                table: "PacketProduct",
                columns: new[] { "PacketsId", "ProductsId" });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2023, 10, 5, 22, 16, 10, 559, DateTimeKind.Local).AddTicks(2567));

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 5, 22, 16, 10, 559, DateTimeKind.Local).AddTicks(2611));

            migrationBuilder.CreateIndex(
                name: "IX_PacketProduct_ProductsId",
                table: "PacketProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PacketProduct_Packets_PacketsId",
                table: "PacketProduct",
                column: "PacketsId",
                principalTable: "Packets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacketProduct_Packets_PacketsId",
                table: "PacketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PacketProduct",
                table: "PacketProduct");

            migrationBuilder.DropIndex(
                name: "IX_PacketProduct_ProductsId",
                table: "PacketProduct");

            migrationBuilder.RenameColumn(
                name: "PacketsId",
                table: "PacketProduct",
                newName: "packetsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PacketProduct",
                table: "PacketProduct",
                columns: new[] { "ProductsId", "packetsId" });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2023, 10, 5, 20, 27, 45, 484, DateTimeKind.Local).AddTicks(1656));

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 5, 20, 27, 45, 484, DateTimeKind.Local).AddTicks(1701));

            migrationBuilder.CreateIndex(
                name: "IX_PacketProduct_packetsId",
                table: "PacketProduct",
                column: "packetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PacketProduct_Packets_packetsId",
                table: "PacketProduct",
                column: "packetsId",
                principalTable: "Packets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
