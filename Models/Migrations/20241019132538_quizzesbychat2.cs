using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class quizzesbychat2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Skill_SkillId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Skill_SkillId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_SkillId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Option_SkillId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Option");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Skill_Id",
                table: "Question",
                column: "Skill_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Option_Skill_Id",
                table: "Option",
                column: "Skill_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Skill_Skill_Id",
                table: "Option",
                column: "Skill_Id",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Skill_Skill_Id",
                table: "Question",
                column: "Skill_Id",
                principalTable: "Skill",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Skill_Skill_Id",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Skill_Skill_Id",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_Skill_Id",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Option_Skill_Id",
                table: "Option");

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Option",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Question_SkillId",
                table: "Question",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_SkillId",
                table: "Option",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Skill_SkillId",
                table: "Option",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Skill_SkillId",
                table: "Question",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
