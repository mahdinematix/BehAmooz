using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UniversityIdFieldAddedToMessageAgg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UniversityId",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Messages");
        }
    }
}
