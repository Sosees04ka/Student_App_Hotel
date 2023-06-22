using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class NullCinferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceBookings_Conferences_ConferenceId",
                table: "ConferenceBookings");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "ConferenceBookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceBookings_Conferences_ConferenceId",
                table: "ConferenceBookings",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceBookings_Conferences_ConferenceId",
                table: "ConferenceBookings");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "ConferenceBookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceBookings_Conferences_ConferenceId",
                table: "ConferenceBookings",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
