using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelDataBaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class coursework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Headwaiters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadwaiterFIO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadwaiterEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadwaiterPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadwaiterLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadwaiterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headwaiters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganiserFIO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganiserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganiserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganiserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganiserNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadwaiterId = table.Column<int>(type: "int", nullable: false),
                    DinnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DinnerPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dinners_Headwaiters_HeadwaiterId",
                        column: x => x.HeadwaiterId,
                        principalTable: "Headwaiters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganiserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conferences_Organisers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "Organisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealPlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealPlanPrice = table.Column<double>(type: "float", nullable: false),
                    OrganiserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_Organisers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "Organisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberFIO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Citizenship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganiserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Organisers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "Organisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadwaiterId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConferenceBookings_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConferenceBookings_Headwaiters_HeadwaiterId",
                        column: x => x.HeadwaiterId,
                        principalTable: "Headwaiters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomFrame = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomPrice = table.Column<double>(type: "float", nullable: false),
                    HeadwaiterId = table.Column<int>(type: "int", nullable: false),
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Headwaiters_HeadwaiterId",
                        column: x => x.HeadwaiterId,
                        principalTable: "Headwaiters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConferenceMembers_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConferenceMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealPlanMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlanMembers_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPlanMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceBookingDinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceBookingId = table.Column<int>(type: "int", nullable: false),
                    DinnerId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DinnercId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceBookingDinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConferenceBookingDinners_ConferenceBookings_ConferenceBookingId",
                        column: x => x.ConferenceBookingId,
                        principalTable: "ConferenceBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConferenceBookingDinners_Dinners_DinnercId",
                        column: x => x.DinnercId,
                        principalTable: "Dinners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomDinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    DinnerId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomDinners_Dinners_DinnerId",
                        column: x => x.DinnerId,
                        principalTable: "Dinners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomDinners_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookingDinners_ConferenceBookingId",
                table: "ConferenceBookingDinners",
                column: "ConferenceBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookingDinners_DinnercId",
                table: "ConferenceBookingDinners",
                column: "DinnercId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookings_ConferenceId",
                table: "ConferenceBookings",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceBookings_HeadwaiterId",
                table: "ConferenceBookings",
                column: "HeadwaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceMembers_ConferenceId",
                table: "ConferenceMembers",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceMembers_MemberId",
                table: "ConferenceMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_OrganiserId",
                table: "Conferences",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dinners_HeadwaiterId",
                table: "Dinners",
                column: "HeadwaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanMembers_MealPlanId",
                table: "MealPlanMembers",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanMembers_MemberId",
                table: "MealPlanMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_OrganiserId",
                table: "MealPlans",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_OrganiserId",
                table: "Members",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDinners_DinnerId",
                table: "RoomDinners",
                column: "DinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDinners_RoomId",
                table: "RoomDinners",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HeadwaiterId",
                table: "Rooms",
                column: "HeadwaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_MealPlanId",
                table: "Rooms",
                column: "MealPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceBookingDinners");

            migrationBuilder.DropTable(
                name: "ConferenceMembers");

            migrationBuilder.DropTable(
                name: "MealPlanMembers");

            migrationBuilder.DropTable(
                name: "RoomDinners");

            migrationBuilder.DropTable(
                name: "ConferenceBookings");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Dinners");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "Headwaiters");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "Organisers");
        }
    }
}
