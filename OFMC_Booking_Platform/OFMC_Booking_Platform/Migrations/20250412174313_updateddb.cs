using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class updateddb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment");

            migrationBuilder.DeleteData(
                table: "Appointment",
                keyColumn: "AppointmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "PatientId",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Patient",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Patient",
                newName: "Password");

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "PatientId", "DOB", "FirstName", "LastName", "Password", "PatientEmail" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sierra", "Erb", "password", "sierraerb25@gmail.com" });

            migrationBuilder.InsertData(
                table: "Appointment",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentEmail", "AppointmentPhone", "ContactMethod", "DoctorId", "Notes", "PatientId", "PatientName" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "sierraerb25@gmail.com", "+15483335882", 0, 1, "Headache", 1, "Sara Hanks" });

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
    }
}
