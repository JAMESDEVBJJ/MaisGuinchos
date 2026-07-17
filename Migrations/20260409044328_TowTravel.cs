using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisGuinchos.Migrations
{
    /// <inheritdoc />
    public partial class TowTravel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TowTravels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TowRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedArrivalTime = table.Column<int>(type: "integer", nullable: false),
                    TowTravelStatus = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TowTravels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TowTravels_TowRequests_TowRequestId",
                        column: x => x.TowRequestId,
                        principalTable: "TowRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TowTravels_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TowTravels_DriverId",
                table: "TowTravels",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TowTravels_TowRequestId",
                table: "TowTravels",
                column: "TowRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TowTravels");
        }
    }
}
