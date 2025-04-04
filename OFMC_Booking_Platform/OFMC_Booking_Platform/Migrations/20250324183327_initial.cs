using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorSpecialty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorExt = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactMethod = table.Column<int>(type: "int", nullable: false),
                    AppointmentEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Doctor",
                columns: new[] { "DoctorId", "DoctorExt", "DoctorName", "DoctorSpecialty" },
                values: new object[,]
                {
                    { 1, 106, "Dr. Emily Carter", "Family Physician" },
                    { 2, 103, "Dr. Shawn Kieze", "Pediatrics" },
                    { 3, 102, "Dr. Sophia Lee", "Women's Health & OB-GYN" },
                    { 4, 104, "Dr. James Thompson", "Internal Medicine" },
                    { 5, 105, "Dr. Olivia Martinez", "Dermatology" },
                    { 6, 101, "Dr. Ryan Patel", "Family Physician" }
                });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "PatientId", "DOB", "FirstName", "LastName", "Password", "PatientEmail" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sierra", "Erb", "password", "sierraerb25@gmail.com" });

            migrationBuilder.InsertData(
                table: "Appointment",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentEmail", "ContactMethod", "DoctorId", "Notes", "PatientId", "PatientName" },
                values: new object[] { 1, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "sierraerb25@gmail.com", 0, 1, "Headache", 1, "Sara Hanks" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Doctor");
        }
    }
}
