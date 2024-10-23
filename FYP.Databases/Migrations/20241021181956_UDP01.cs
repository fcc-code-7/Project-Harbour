using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UDP01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateofCreation",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "areaofintrest",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateofCreation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "areaofintrest",
                table: "AspNetUsers");
        }
    }
}
