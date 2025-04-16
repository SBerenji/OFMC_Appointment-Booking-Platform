using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OFMC_Booking_Platform.Migrations
{
    /// <inheritdoc />
    public partial class newdoctorwitholdappointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 19,
                column: "DoctorId",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "SlotId",
                keyValue: 19,
                column: "DoctorId",
                value: 1);
        }
    }
}
