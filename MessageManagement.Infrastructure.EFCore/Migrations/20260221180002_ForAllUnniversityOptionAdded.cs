using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ForAllUnniversityOptionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForAllUniversities",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForAllUniversities",
                table: "Messages");
        }
    }
}
