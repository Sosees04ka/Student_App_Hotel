using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class plswork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "ConferenceBookingDinners");

            migrationBuilder.AddColumn<string>(
                name: "NameHall",
                table: "ConferenceBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameHall",
                table: "ConferenceBookingDinners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameHall",
                table: "ConferenceBookings");

            migrationBuilder.DropColumn(
                name: "NameHall",
                table: "ConferenceBookingDinners");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ConferenceBookingDinners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
