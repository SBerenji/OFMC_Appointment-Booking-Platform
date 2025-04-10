using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class newavailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "SlotId", "DoctorId", "IsBooked", "SlotDateTime" },
                values: new object[,]
                {
                    { 10, 4, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 4, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 4, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 5, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 5, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 5, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 6, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 6, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 6, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 18);
        }
    }
}
