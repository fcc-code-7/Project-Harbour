using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class Removefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.RenameColumn(
                name: "PropDefID",
                table: "Projects",
                newName: "projectGroup");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedResults",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Others",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCategory",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tools",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "commiteeId",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "companyID",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "groupId",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "objectives",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedResults",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Others",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectCategory",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Tools",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "code",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "commiteeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "companyID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "groupId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "objectives",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "projectGroup",
                table: "Projects",
                newName: "PropDefID");

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExpectedResults = table.Column<string>(type: "TEXT", nullable: false),
                    Others = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectCategory = table.Column<string>(type: "TEXT", nullable: false),
                    Specialization = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Tools = table.Column<string>(type: "TEXT", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    commiteeId = table.Column<string>(type: "TEXT", nullable: false),
                    companyID = table.Column<string>(type: "TEXT", nullable: false),
                    groupId = table.Column<string>(type: "TEXT", nullable: false),
                    objectives = table.Column<string>(type: "TEXT", nullable: false),
                    projectGroup = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.ID);
                });
        }
    }
}
