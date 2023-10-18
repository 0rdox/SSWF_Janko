using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class addedCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products",
                column: "PacketId",
                principalTable: "Packets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products",
                column: "PacketId",
                principalTable: "Packets",
                principalColumn: "Id");
        }
    }
}
