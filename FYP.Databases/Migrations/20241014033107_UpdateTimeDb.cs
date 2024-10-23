using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lapse",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "RoomIncharge",
                table: "FYPCommittes");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Rooms",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Lapse",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomIncharge",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);
        }
    }
}
