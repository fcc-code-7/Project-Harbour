using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropColumn(
                name: "Batch",
                table: "RoomInCharge");

            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "RoomInCharge");

            migrationBuilder.DropColumn(
                name: "RoomAlloted",
                table: "RoomInCharge");

            migrationBuilder.CreateTable(
                name: "RoomAllotments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    InchargeId = table.Column<string>(type: "TEXT", nullable: true),
                    Room = table.Column<string>(type: "TEXT", nullable: true),
                    Batch = table.Column<string>(type: "TEXT", nullable: true),
                    Evaluation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAllotments", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomAllotments");

            migrationBuilder.AddColumn<string>(
                name: "Batch",
                table: "RoomInCharge",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Evaluation",
                table: "RoomInCharge",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomAlloted",
                table: "RoomInCharge",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Designation = table.Column<string>(type: "TEXT", nullable: true),
                    SupervisorID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.ID);
                });
        }
    }
}
