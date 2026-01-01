using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalEmployeeApp.Migrations
{
    /// <inheritdoc />
    public partial class vehicledamage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "VehicleDamage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "VehicleDamage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
