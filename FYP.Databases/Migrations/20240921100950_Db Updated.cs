using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropDocs",
                table: "Projects",
                newName: "PropReport");

            migrationBuilder.RenameColumn(
                name: "MidDocs",
                table: "Projects",
                newName: "PropPPT");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "WeeklyLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MidPPT",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MidReport",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "WeeklyLogs");

            migrationBuilder.DropColumn(
                name: "MidPPT",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MidReport",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "PropReport",
                table: "Projects",
                newName: "PropDocs");

            migrationBuilder.RenameColumn(
                name: "PropPPT",
                table: "Projects",
                newName: "MidDocs");
        }
    }
}
