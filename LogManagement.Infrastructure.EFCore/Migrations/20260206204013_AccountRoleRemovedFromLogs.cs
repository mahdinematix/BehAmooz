using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AccountRoleRemovedFromLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountRole",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountRole",
                table: "Logs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
