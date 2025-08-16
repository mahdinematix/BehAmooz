using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UniTypePropAddedToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniversityType",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityType",
                table: "Courses");
        }
    }
}
