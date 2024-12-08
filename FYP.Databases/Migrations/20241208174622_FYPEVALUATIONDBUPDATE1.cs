using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class FYPEVALUATIONDBUPDATE1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CordinatorMarks",
                table: "EvaluationCriterias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorMarks",
                table: "EvaluationCriterias",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CordinatorMarks",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "SupervisorMarks",
                table: "EvaluationCriterias");
        }
    }
}
