using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ProfessorIdPropertyAddedToClassModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfessorId",
                table: "Classes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Classes");
        }
    }
}
