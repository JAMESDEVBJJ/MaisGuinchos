using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisGuinchos.Migrations
{
    /// <inheritdoc />
    public partial class SepararCamposDasDuasRotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DistanceToDestinationKm",
                table: "TowTravels",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceToPickupKm",
                table: "TowTravels",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinToDestination",
                table: "TowTravels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinToPickup",
                table: "TowTravels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDistanceKm",
                table: "TowTravels",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceToDestinationKm",
                table: "TowRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceToPickupKm",
                table: "TowRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinToDestination",
                table: "TowRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinToPickup",
                table: "TowRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceToDestinationKm",
                table: "TowTravels");

            migrationBuilder.DropColumn(
                name: "DistanceToPickupKm",
                table: "TowTravels");

            migrationBuilder.DropColumn(
                name: "DurationMinToDestination",
                table: "TowTravels");

            migrationBuilder.DropColumn(
                name: "DurationMinToPickup",
                table: "TowTravels");

            migrationBuilder.DropColumn(
                name: "TotalDistanceKm",
                table: "TowTravels");

            migrationBuilder.DropColumn(
                name: "DistanceToDestinationKm",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "DistanceToPickupKm",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "DurationMinToDestination",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "DurationMinToPickup",
                table: "TowRequests");
        }
    }
}
