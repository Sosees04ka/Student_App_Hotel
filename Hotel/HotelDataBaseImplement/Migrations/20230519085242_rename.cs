using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ConferenceBookings",
                newName: "BookingDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "ConferenceBookings",
                newName: "StartDate");
        }
    }
}
