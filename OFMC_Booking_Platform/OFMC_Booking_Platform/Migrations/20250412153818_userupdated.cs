using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class userupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DeleteData(
                table: "Appointment",
                keyColumn: "AppointmentId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Appointment");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                });

            migrationBuilder.InsertData(
                table: "Appointment",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentEmail", "AppointmentPhone", "ContactMethod", "DoctorId", "Notes", "PatientId", "PatientName" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "sierraerb25@gmail.com", "+15483335882", 0, 1, "Headache", 1, "Sara Hanks" });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "PatientId", "DOB", "FirstName", "LastName", "Password", "PatientEmail" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sierra", "Erb", "password", "sierraerb25@gmail.com" });
        }
    }
}
