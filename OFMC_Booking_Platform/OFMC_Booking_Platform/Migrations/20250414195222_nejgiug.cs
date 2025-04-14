using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class nejgiug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "SlotId", "DoctorId", "IsBooked", "SlotDateTime" },
                values: new object[,]
                {
                    { 19, 1, false, new DateTime(2025, 1, 28, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, false, new DateTime(2025, 8, 15, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, false, new DateTime(2025, 9, 12, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1, false, new DateTime(2025, 8, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 1, false, new DateTime(2025, 8, 12, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 1, false, new DateTime(2025, 8, 13, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 1, false, new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 1, false, new DateTime(2025, 8, 15, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1, false, new DateTime(2025, 8, 16, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 27);
        }
    }
}
