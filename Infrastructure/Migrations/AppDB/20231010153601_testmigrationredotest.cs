using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class testmigrationredotest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packets_PacketId1",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "PacketId1",
                table: "Products",
                newName: "DemoProductsId1");

            migrationBuilder.RenameIndex(
                name: "IX_Products_PacketId1",
                table: "Products",
                newName: "IX_Products_DemoProductsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DemoProducts_DemoProductsId1",
                table: "Products",
                column: "DemoProductsId1",
                principalTable: "DemoProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_DemoProducts_DemoProductsId1",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DemoProductsId1",
                table: "Products",
                newName: "PacketId1");

            migrationBuilder.RenameIndex(
                name: "IX_Products_DemoProductsId1",
                table: "Products",
                newName: "IX_Products_PacketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packets_PacketId1",
                table: "Products",
                column: "PacketId1",
                principalTable: "Packets",
                principalColumn: "Id");
        }
    }
}
