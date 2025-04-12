using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class newdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 1,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 2,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 3,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 4,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 5,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 6,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 7,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 8,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 9,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 10,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 11,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 12,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 13,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 14,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 15,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 16,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 17,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 18,
                column: "SlotDateTime",
                value: new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment");

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 1,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 2,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 3,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 4,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 5,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 6,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 7,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 8,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 9,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 10,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 11,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 12,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 13,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 14,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 15,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 16,
                column: "SlotDateTime",
                value: new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 17,
                column: "SlotDateTime",
                value: new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 18,
                column: "SlotDateTime",
                value: new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
