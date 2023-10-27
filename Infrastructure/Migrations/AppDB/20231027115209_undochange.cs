using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    /// <inheritdoc />
    public partial class undochange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupTime",
                table: "Packets",
                newName: "MaxDateTime");

            migrationBuilder.RenameColumn(
                name: "MaxPickupTime",
                table: "Packets",
                newName: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxDateTime",
                table: "Packets",
                newName: "PickupTime");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Packets",
                newName: "MaxPickupTime");
        }
    }
}
