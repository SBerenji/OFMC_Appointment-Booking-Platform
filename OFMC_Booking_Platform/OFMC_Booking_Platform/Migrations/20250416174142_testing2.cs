using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class testing2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 22,
                column: "SlotDateTime",
                value: new DateTime(2025, 8, 11, 12, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 22,
                column: "SlotDateTime",
                value: new DateTime(2025, 8, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
