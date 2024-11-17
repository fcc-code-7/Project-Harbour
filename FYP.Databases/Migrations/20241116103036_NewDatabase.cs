using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PId",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q1",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q1Desc",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q2",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q2Desc",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q4",
                table: "EvaluationCriterias");

            migrationBuilder.RenameColumn(
                name: "Q5Desc",
                table: "EvaluationCriterias",
                newName: "Q8Marks");

            migrationBuilder.RenameColumn(
                name: "Q5",
                table: "EvaluationCriterias",
                newName: "Q7Marks");

            migrationBuilder.RenameColumn(
                name: "Q4Desc",
                table: "EvaluationCriterias",
                newName: "Q6Marks");

            migrationBuilder.RenameColumn(
                name: "Q3Desc",
                table: "EvaluationCriterias",
                newName: "GId");

            migrationBuilder.RenameColumn(
                name: "Q3",
                table: "EvaluationCriterias",
                newName: "Batch");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Q8Marks",
                table: "EvaluationCriterias",
                newName: "Q5Desc");

            migrationBuilder.RenameColumn(
                name: "Q7Marks",
                table: "EvaluationCriterias",
                newName: "Q5");

            migrationBuilder.RenameColumn(
                name: "Q6Marks",
                table: "EvaluationCriterias",
                newName: "Q4Desc");

            migrationBuilder.RenameColumn(
                name: "GId",
                table: "EvaluationCriterias",
                newName: "Q3Desc");

            migrationBuilder.RenameColumn(
                name: "Batch",
                table: "EvaluationCriterias",
                newName: "Q3");

            migrationBuilder.AddColumn<string>(
                name: "PId",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q1",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q1Desc",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q2",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q2Desc",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q4",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
