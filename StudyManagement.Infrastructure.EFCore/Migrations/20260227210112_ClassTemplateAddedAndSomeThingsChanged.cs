using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ClassTemplateAddedAndSomeThingsChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Classes_ClassId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CourseId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Classes");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'ClassTemplateId') IS NOT NULL
   AND COL_LENGTH('dbo.OrderItems', 'ClassId') IS NULL
BEGIN
    EXEC sp_rename 'dbo.OrderItems.ClassTemplateId', 'ClassId', 'COLUMN';
END
");

            migrationBuilder.RenameColumn(
                name: "ProfessorId",
                table: "Classes",
                newName: "ClassTemplateId");

            migrationBuilder.CreateTable(
                name: "ClassTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessorId = table.Column<long>(type: "bigint", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassTemplates_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassTemplateId",
                table: "Classes",
                column: "ClassTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTemplates_CourseId_ProfessorId",
                table: "ClassTemplates",
                columns: new[] { "CourseId", "ProfessorId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_ClassTemplates_ClassTemplateId",
                table: "Classes",
                column: "ClassTemplateId",
                principalTable: "ClassTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_ClassTemplates_ClassTemplateId",
                table: "Sessions",
                column: "ClassTemplateId",
                principalTable: "ClassTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_ClassTemplates_ClassTemplateId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_ClassTemplates_ClassTemplateId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "ClassTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassTemplateId",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "CurrentSemesterId",
                table: "Universities",
                newName: "CurrentSemesterCode");

            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.OrderItems', 'ClassId') IS NOT NULL
   AND COL_LENGTH('dbo.OrderItems', 'ClassTemplateId') IS NULL
BEGIN
    EXEC sp_rename 'dbo.OrderItems.ClassId', 'ClassTemplateId', 'COLUMN';
END
");

            migrationBuilder.RenameColumn(
                name: "ClassTemplateId",
                table: "Classes",
                newName: "ProfessorId");

            migrationBuilder.AddColumn<long>(
                name: "CourseId",
                table: "Classes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Classes_ClassTemplateId",
                table: "Sessions",
                column: "ClassTemplateId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
