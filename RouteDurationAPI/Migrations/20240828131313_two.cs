using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RouteDurationAPI.Migrations
{
    /// <inheritdoc />
    public partial class two : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "VehicleEntries",
                newName: "StartTimestamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "StopTimestamp",
                table: "VehicleEntries",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StopTimestamp",
                table: "VehicleEntries");

            migrationBuilder.RenameColumn(
                name: "StartTimestamp",
                table: "VehicleEntries",
                newName: "Timestamp");
        }
    }
}
