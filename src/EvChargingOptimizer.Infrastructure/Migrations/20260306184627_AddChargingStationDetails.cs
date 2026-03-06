using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvChargingOptimizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChargingStationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectorType",
                table: "ChargingStations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PowerCapacityKw",
                table: "ChargingStations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerKwh",
                table: "ChargingStations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectorType",
                table: "ChargingStations");

            migrationBuilder.DropColumn(
                name: "PowerCapacityKw",
                table: "ChargingStations");

            migrationBuilder.DropColumn(
                name: "PricePerKwh",
                table: "ChargingStations");
        }
    }
}
