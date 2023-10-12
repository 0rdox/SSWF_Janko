using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class canteenfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Canteens_CanteenID",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "CanteenID",
                table: "Employees",
                newName: "CanteenId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_CanteenID",
                table: "Employees",
                newName: "IX_Employees_CanteenId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Canteens_Location",
                table: "Canteens",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Packets_Canteen",
                table: "Packets",
                column: "Canteen");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Canteens_CanteenId",
                table: "Employees",
                column: "CanteenId",
                principalTable: "Canteens",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packets_Canteens_Canteen",
                table: "Packets",
                column: "Canteen",
                principalTable: "Canteens",
                principalColumn: "Location",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Canteens_CanteenId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Packets_Canteens_Canteen",
                table: "Packets");

            migrationBuilder.DropIndex(
                name: "IX_Packets_Canteen",
                table: "Packets");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Canteens_Location",
                table: "Canteens");

            migrationBuilder.RenameColumn(
                name: "CanteenId",
                table: "Employees",
                newName: "CanteenID");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_CanteenId",
                table: "Employees",
                newName: "IX_Employees_CanteenID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Canteens_CanteenID",
                table: "Employees",
                column: "CanteenID",
                principalTable: "Canteens",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
