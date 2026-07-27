using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisGuinchos.Migrations
{
    /// <inheritdoc />
    public partial class AddCounterOfferFieldsToTowRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CounterOfferAt",
                table: "TowRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounterOfferDriverId",
                table: "TowRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CounterOfferPercent",
                table: "TowRequests",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CounterOfferPrice",
                table: "TowRequests",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounterOfferReason",
                table: "TowRequests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CounterOfferAt",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "CounterOfferDriverId",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "CounterOfferPercent",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "CounterOfferPrice",
                table: "TowRequests");

            migrationBuilder.DropColumn(
                name: "CounterOfferReason",
                table: "TowRequests");
        }
    }
}
