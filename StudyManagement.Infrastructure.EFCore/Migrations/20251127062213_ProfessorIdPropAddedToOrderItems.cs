using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ProfessorIdPropAddedToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfessorId",
                table: "OrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "OrderItems");
        }
    }
}
