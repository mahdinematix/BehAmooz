using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UniTypeIdAddedToMessageAgg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniversityTypeId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityTypeId",
                table: "Messages");
        }
    }
}
