using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class updatedb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "WeeklyLogs",
                newName: "SubmitDate");

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "WeeklyLogs",
                newName: "WeekNo");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WeeklyLogs",
                newName: "AssignDate");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "WeeklyLogs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "WeeklyLogs");

            migrationBuilder.RenameColumn(
                name: "WeekNo",
                table: "WeeklyLogs",
                newName: "GroupName");

            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "WeeklyLogs",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "AssignDate",
                table: "WeeklyLogs",
                newName: "Date");
        }
    }
}
