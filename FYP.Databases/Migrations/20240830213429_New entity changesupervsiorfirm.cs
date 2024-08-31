using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYP.Databases.Migrations
{
    /// <inheritdoc />
    public partial class Newentitychangesupervsiorfirm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChangeTitleForms",
                table: "ChangeTitleForms");

            migrationBuilder.RenameTable(
                name: "ChangeTitleForms",
                newName: "ChangeTitleForm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChangeTitleForm",
                table: "ChangeTitleForm",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "changeSupervisorForms",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    NewSupervsiorId = table.Column<string>(type: "TEXT", nullable: true),
                    oldsupervisorId = table.Column<string>(type: "TEXT", nullable: true),
                    GroupId = table.Column<string>(type: "TEXT", nullable: true),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    OtherReason = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changeSupervisorForms", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "changeSupervisorForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChangeTitleForm",
                table: "ChangeTitleForm");

            migrationBuilder.RenameTable(
                name: "ChangeTitleForm",
                newName: "ChangeTitleForms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChangeTitleForms",
                table: "ChangeTitleForms",
                column: "ID");
        }
    }
}
