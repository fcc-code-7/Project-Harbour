using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class NewChangesinWeeklyLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppointedDate",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointedTime",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EvaluationID",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomIncharge",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointedDate",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "AppointedTime",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "EvaluationID",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "RoomIncharge",
                table: "FYPCommittes");
        }
    }
}
