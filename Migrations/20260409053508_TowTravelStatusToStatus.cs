using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisGuinchos.Migrations
{
    /// <inheritdoc />
    public partial class TowTravelStatusToStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TowTravelStatus",
                table: "TowTravels",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TowTravels",
                newName: "TowTravelStatus");
        }
    }
}
