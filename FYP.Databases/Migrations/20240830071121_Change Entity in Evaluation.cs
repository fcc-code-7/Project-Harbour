using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityinEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PDefId",
                table: "Evaluations",
                newName: "PId");

            migrationBuilder.RenameColumn(
                name: "EvaluationID",
                table: "Evaluations",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "EvaluationName",
                table: "Evaluations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluationName",
                table: "Evaluations");

            migrationBuilder.RenameColumn(
                name: "PId",
                table: "Evaluations",
                newName: "PDefId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Evaluations",
                newName: "EvaluationID");
        }
    }
}
