using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityinEvaluation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommiteeID",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Evaluations");

            migrationBuilder.AddColumn<string>(
                name: "CommiteeID",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommiteeID",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "EvaluationCriterias");

            migrationBuilder.AddColumn<string>(
                name: "CommiteeID",
                table: "Evaluations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Evaluations",
                type: "TEXT",
                nullable: true);
        }
    }
}
