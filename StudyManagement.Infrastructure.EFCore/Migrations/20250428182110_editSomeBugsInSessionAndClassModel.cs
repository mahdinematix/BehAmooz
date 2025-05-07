using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class editSomeBugsInSessionAndClassModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CourseId",
                table: "Sessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
