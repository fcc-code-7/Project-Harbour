using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedInitials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Companies",
                newName: "MentorTelephone");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Companies",
                newName: "MentorName");

            migrationBuilder.AddColumn<string>(
                name: "batch",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MentorEmail",
                table: "Companies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "batch",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MentorEmail",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "MentorTelephone",
                table: "Companies",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "MentorName",
                table: "Companies",
                newName: "Email");
        }
    }
}
