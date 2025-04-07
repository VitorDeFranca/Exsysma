using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExsysmaAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class mudancaClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facts");

            migrationBuilder.RenameColumn(
                name: "Condicao",
                table: "Rules",
                newName: "GoalVariableValue");

            migrationBuilder.RenameColumn(
                name: "Acao",
                table: "Rules",
                newName: "Conditions");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Projects",
                newName: "Responsible");

            migrationBuilder.AddColumn<int>(
                name: "GoalVariableId",
                table: "Rules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionDescription = table.Column<string>(type: "TEXT", nullable: true),
                    IsGoalVariable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Values = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variables_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rules_GoalVariableId",
                table: "Rules",
                column: "GoalVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_ProjectId",
                table: "Variables",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Variables_GoalVariableId",
                table: "Rules",
                column: "GoalVariableId",
                principalTable: "Variables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Variables_GoalVariableId",
                table: "Rules");

            migrationBuilder.DropTable(
                name: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Rules_GoalVariableId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "GoalVariableId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "GoalVariableValue",
                table: "Rules",
                newName: "Condicao");

            migrationBuilder.RenameColumn(
                name: "Conditions",
                table: "Rules",
                newName: "Acao");

            migrationBuilder.RenameColumn(
                name: "Responsible",
                table: "Projects",
                newName: "Nome");

            migrationBuilder.CreateTable(
                name: "Facts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facts_ProjectId",
                table: "Facts",
                column: "ProjectId");
        }
    }
}
