using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllotedDate",
                table: "RoomInCharge",
                newName: "Evaluation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Evaluation",
                table: "RoomInCharge",
                newName: "AllotedDate");
        }
    }
}
