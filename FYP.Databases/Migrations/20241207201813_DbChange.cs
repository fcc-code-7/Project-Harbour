using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DbChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Q8Marks",
                table: "EvaluationCriterias",
                newName: "StudentsProposalMarks");

            migrationBuilder.RenameColumn(
                name: "Q7Marks",
                table: "EvaluationCriterias",
                newName: "Student3MidMarks");

            migrationBuilder.RenameColumn(
                name: "Q6Marks",
                table: "EvaluationCriterias",
                newName: "Student3FinalMarks");

            migrationBuilder.RenameColumn(
                name: "Q5Marks",
                table: "EvaluationCriterias",
                newName: "Student2MidMarks");

            migrationBuilder.RenameColumn(
                name: "Q4Marks",
                table: "EvaluationCriterias",
                newName: "Student2FinalMarks");

            migrationBuilder.RenameColumn(
                name: "Q3Marks",
                table: "EvaluationCriterias",
                newName: "Student1MidMarks");

            migrationBuilder.RenameColumn(
                name: "Q2Marks",
                table: "EvaluationCriterias",
                newName: "Student1FinalMarks");

            migrationBuilder.RenameColumn(
                name: "Q1Marks",
                table: "EvaluationCriterias",
                newName: "Q8");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "FYPCommittes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q1",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q2",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q3",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q4",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q5",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q6",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Q7",
                table: "EvaluationCriterias",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "FYPCommittes");

            migrationBuilder.DropColumn(
                name: "Q1",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q2",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q3",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q4",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q5",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q6",
                table: "EvaluationCriterias");

            migrationBuilder.DropColumn(
                name: "Q7",
                table: "EvaluationCriterias");

            migrationBuilder.RenameColumn(
                name: "StudentsProposalMarks",
                table: "EvaluationCriterias",
                newName: "Q8Marks");

            migrationBuilder.RenameColumn(
                name: "Student3MidMarks",
                table: "EvaluationCriterias",
                newName: "Q7Marks");

            migrationBuilder.RenameColumn(
                name: "Student3FinalMarks",
                table: "EvaluationCriterias",
                newName: "Q6Marks");

            migrationBuilder.RenameColumn(
                name: "Student2MidMarks",
                table: "EvaluationCriterias",
                newName: "Q5Marks");

            migrationBuilder.RenameColumn(
                name: "Student2FinalMarks",
                table: "EvaluationCriterias",
                newName: "Q4Marks");

            migrationBuilder.RenameColumn(
                name: "Student1MidMarks",
                table: "EvaluationCriterias",
                newName: "Q3Marks");

            migrationBuilder.RenameColumn(
                name: "Student1FinalMarks",
                table: "EvaluationCriterias",
                newName: "Q2Marks");

            migrationBuilder.RenameColumn(
                name: "Q8",
                table: "EvaluationCriterias",
                newName: "Q1Marks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);
        }
    }
}
