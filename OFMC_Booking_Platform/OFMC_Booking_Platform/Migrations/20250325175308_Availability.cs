using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    SlotDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.SlotId);
                });

            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "SlotId", "DoctorId", "IsBooked", "SlotDateTime" },
                values: new object[,]
                {
                    { 1, 1, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 2, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 3, false, new DateTime(2022, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 3, false, new DateTime(2023, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 3, false, new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availability");
        }
    }
}
