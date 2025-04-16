using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class newdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 26,
                column: "SlotDateTime",
                value: new DateTime(2025, 8, 15, 2, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 26,
                column: "SlotDateTime",
                value: new DateTime(2025, 8, 15, 12, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
