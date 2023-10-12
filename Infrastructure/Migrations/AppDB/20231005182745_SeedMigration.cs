using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class SeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Canteens_CanteenID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Packets_Canteens_CanteenID",
                table: "Packets");

            migrationBuilder.DropForeignKey(
                name: "FK_Packets_Students_ReservedById",
                table: "Packets");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PacketId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Packets_CanteenID",
                table: "Packets");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CanteenID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PacketId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CanteenID",
                table: "Packets",
                newName: "Canteen");

            migrationBuilder.RenameColumn(
                name: "CanteenID",
                table: "Employees",
                newName: "Canteen");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedById",
                table: "Packets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Packets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "Canteens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PacketProduct",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    packetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacketProduct", x => new { x.ProductsId, x.packetsId });
                    table.ForeignKey(
                        name: "FK_PacketProduct_Packets_packetsId",
                        column: x => x.packetsId,
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

            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "ID", "City", "Location", "OffersHotMeals" },
                values: new object[,]
                {
                    { 1, 0, 0, false },
                    { 2, 2, 2, true },
                    { 3, 0, 1, true }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeID", "Canteen", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Derek Stor" },
                    { 2, 0, "Jenny Berk" },
                    { 3, 2, "Sarah Lee" },
                    { 4, 1, "Dom Peters" }
                });

            migrationBuilder.InsertData(
                table: "Packets",
                columns: new[] { "Id", "Canteen", "City", "DateTime", "ImageUrl", "MaxDateTime", "Name", "OverEighteen", "Price", "ReservedById", "Type" },
                values: new object[,]
                {
                    { 1, 0, 0, new DateTime(2023, 10, 5, 20, 27, 45, 484, DateTimeKind.Local).AddTicks(1656), "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lekkere Broodjes", false, 8.99m, null, 0 },
                    { 2, 0, 0, new DateTime(2023, 10, 5, 20, 27, 45, 484, DateTimeKind.Local).AddTicks(1701), "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drankpakket", false, 14.99m, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Alcohol", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, false, "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg", "Worstenbroodje" },
                    { 2, false, "https://www.bakkerijvoortman.nl/wp/wp-content/uploads/2015/02/saucbr..png", "Saucijzenbroodje" },
                    { 3, false, "https://www.ahealthylife.nl/wp-content/uploads/2021/06/Kaassouffle_voedingswaarde-1.jpg", "Kaassoufflé" },
                    { 4, false, "https://klokhoogeveen.nl/wordpress/wp-content/uploads/2019/04/frikandellenbroodje.png\"", "Frikandelbroodje" },
                    { 5, false, "https://i0.wp.com/www.frituurwereld.nl/wp-content/uploads/2020/10/Frituurwereld-Krokettendag-hoera.jpg?fit=1212%2C823&ssl=1", "Kroket" },
                    { 6, false, "https://s-marktscholte.nl/cmcc/wp-content/uploads/2019/01/Broodje-gezond-s-markt.jpg", "Pistoletje gezond" },
                    { 7, false, "https://us.123rf.com/450wm/kostrez/kostrez1509/kostrez150900011/44695927-tomatensoep-in-een-witte-kom-met-peterselie-en-specerijen-op-een-schotel-ge%C3%AFsoleerd-op-een-witte.jpg?ver=6", "Tomatensoep" },
                    { 8, false, "https://img.freepik.com/premium-photo/chocolade-op-witte-achtergrond_404043-1540.jpg", "Chocolade" },
                    { 9, false, "https://media.istockphoto.com/id/1251818705/photo/large-chocolate-chip-cookie-on-a-white-plate-with-a-white-background.jpg?s=612x612&w=0&k=20&c=vDhGIb54eGfIQKI1gfuDGp7j29ppw3ioTa-Wwwrg3Vc=", "Koekje" },
                    { 10, true, "https://thumbs.dreamstime.com/b/koud-bier-43280582.jpg", "Bier" },
                    { 11, true, "https://static.vecteezy.com/ti/gratis-vector/p3/7324461-wijnglazen-met-witte-wijn-illustratie-van-wijnglazen-geisoleerd-op-witte-achtergrond-gratis-vector.jpg", "Wijn" },
                    { 12, true, "https://img.freepik.com/premium-photo/glas-schotse-whisky-en-ijs-op-een-witte-achtergrond_38145-1376.jpg?w=2000", "Whisky" },
                    { 13, true, "https://media.istockphoto.com/id/671705556/photo/vodka-in-vintage-glass.jpg?s=612x612&w=0&k=20&c=Tz58z7MkbFq2ziaK92nid4KEp84T30pFlS6J6pAmI08=", "Vodka" },
                    { 14, true, "https://img.freepik.com/premium-photo/cocktail-tequila-sunrise-front-white-background_118454-21367.jpg?w=2000", "Tequila Sunrise" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateOfBirth", "Email", "Name", "Phone", "StudentNumber" },
                values: new object[] { new DateTime(1998, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "mike.evars@example.com", "Mike Evars", "555-555-5555", 52341 });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "City", "DateOfBirth", "Email", "Name", "Phone", "StudentNumber" },
                values: new object[] { 3, 0, new DateTime(2001, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "kaitlyn.marek@example.com", "Kaitlyn Marek", "555-555-5555", 51221 });

            migrationBuilder.CreateIndex(
                name: "IX_PacketProduct_packetsId",
                table: "PacketProduct",
                column: "packetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packets_Students_ReservedById",
                table: "Packets",
                column: "ReservedById",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packets_Students_ReservedById",
                table: "Packets");

            migrationBuilder.DropTable(
                name: "PacketProduct");

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Packets");

            migrationBuilder.RenameColumn(
                name: "Canteen",
                table: "Packets",
                newName: "CanteenID");

            migrationBuilder.RenameColumn(
                name: "Canteen",
                table: "Employees",
                newName: "CanteenID");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "PacketId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReservedById",
                table: "Packets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Canteens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateOfBirth", "Email", "Name", "Phone", "StudentNumber" },
                values: new object[] { new DateTime(1998, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Jane Smith", "555-555-1234", 67890 });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PacketId",
                table: "Products",
                column: "PacketId");

            migrationBuilder.CreateIndex(
                name: "IX_Packets_CanteenID",
                table: "Packets",
                column: "CanteenID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CanteenID",
                table: "Employees",
                column: "CanteenID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Canteens_CanteenID",
                table: "Employees",
                column: "CanteenID",
                principalTable: "Canteens",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packets_Canteens_CanteenID",
                table: "Packets",
                column: "CanteenID",
                principalTable: "Canteens",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packets_Students_ReservedById",
                table: "Packets",
                column: "ReservedById",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packets_PacketId",
                table: "Products",
                column: "PacketId",
                principalTable: "Packets",
                principalColumn: "Id");
        }
    }
}
