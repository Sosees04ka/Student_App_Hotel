using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class plswork2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceBookingDinners_Dinners_DinnercId",
                table: "ConferenceBookingDinners");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceBookingDinners_DinnercId",
                table: "ConferenceBookingDinners");

            migrationBuilder.DropColumn(
                name: "DinnercId",
                table: "ConferenceBookingDinners");

            migrationBuilder.DropColumn(
                name: "NameHall",
                table: "ConferenceBookingDinners");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookingDinners_DinnerId",
                table: "ConferenceBookingDinners",
                column: "DinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceBookingDinners_Dinners_DinnerId",
                table: "ConferenceBookingDinners",
                column: "DinnerId",
                principalTable: "Dinners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceBookingDinners_Dinners_DinnerId",
                table: "ConferenceBookingDinners");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceBookingDinners_DinnerId",
                table: "ConferenceBookingDinners");

            migrationBuilder.AddColumn<int>(
                name: "DinnercId",
                table: "ConferenceBookingDinners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NameHall",
                table: "ConferenceBookingDinners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookingDinners_DinnercId",
                table: "ConferenceBookingDinners",
                column: "DinnercId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceBookingDinners_Dinners_DinnercId",
                table: "ConferenceBookingDinners",
                column: "DinnercId",
                principalTable: "Dinners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
