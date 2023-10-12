using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class ChangedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PacketProduct");

            migrationBuilder.AddColumn<int>(
                name: "PacketID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTime", "ReservedById" },
                values: new object[] { new DateTime(2023, 10, 6, 10, 44, 14, 99, DateTimeKind.Local).AddTicks(9078), 1 });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 6, 10, 44, 14, 99, DateTimeKind.Local).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "PacketID" },
                values: new object[] { "https://klokhoogeveen.nl/wordpress/wp-content/uploads/2019/04/frikandellenbroodje.png", null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "PacketID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "PacketID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PacketID",
                table: "Products",
                column: "PacketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packets_PacketID",
                table: "Products",
                column: "PacketID",
                principalTable: "Packets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packets_PacketID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PacketID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PacketID",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "PacketProduct",
                columns: table => new
                {
                    PacketsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacketProduct", x => new { x.PacketsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_PacketProduct_Packets_PacketsId",
                        column: x => x.PacketsId,
                        principalTable: "Packets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PacketProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTime", "ReservedById" },
                values: new object[] { new DateTime(2023, 10, 5, 22, 16, 10, 559, DateTimeKind.Local).AddTicks(2567), null });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2023, 10, 5, 22, 16, 10, 559, DateTimeKind.Local).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://klokhoogeveen.nl/wordpress/wp-content/uploads/2019/04/frikandellenbroodje.png\"");

            migrationBuilder.CreateIndex(
                name: "IX_PacketProduct_ProductsId",
                table: "PacketProduct",
                column: "ProductsId");
        }
    }
}
