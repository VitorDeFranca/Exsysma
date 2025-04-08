using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExsysmaAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class mudancaclasserule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Variables_GoalVariableId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "GoalVariableValue",
                table: "Rules");

            migrationBuilder.RenameColumn(
                name: "Values",
                table: "Variables",
                newName: "PossibleValues");

            migrationBuilder.RenameColumn(
                name: "GoalVariableId",
                table: "Rules",
                newName: "ConclusionRuleItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_GoalVariableId",
                table: "Rules",
                newName: "IX_Rules_ConclusionRuleItemId");

            migrationBuilder.CreateTable(
                name: "RuleItem",
                columns: table => new
                {
                    RuleItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VariableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Operator = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    RuleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleItem", x => x.RuleItemId);
                    table.ForeignKey(
                        name: "FK_RuleItem_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RuleItem_Variables_VariableId",
                        column: x => x.VariableId,
                        principalTable: "Variables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuleItem_RuleId",
                table: "RuleItem",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleItem_VariableId",
                table: "RuleItem",
                column: "VariableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_RuleItem_ConclusionRuleItemId",
                table: "Rules",
                column: "ConclusionRuleItemId",
                principalTable: "RuleItem",
                principalColumn: "RuleItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_RuleItem_ConclusionRuleItemId",
                table: "Rules");

            migrationBuilder.DropTable(
                name: "RuleItem");

            migrationBuilder.RenameColumn(
                name: "PossibleValues",
                table: "Variables",
                newName: "Values");

            migrationBuilder.RenameColumn(
                name: "ConclusionRuleItemId",
                table: "Rules",
                newName: "GoalVariableId");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_ConclusionRuleItemId",
                table: "Rules",
                newName: "IX_Rules_GoalVariableId");

            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "Rules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GoalVariableValue",
                table: "Rules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Variables_GoalVariableId",
                table: "Rules",
                column: "GoalVariableId",
                principalTable: "Variables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
