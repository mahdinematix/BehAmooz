using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class IsPayedPropAddedToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayAmount",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "IsPayed",
                table: "Sessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClassDay",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClassEndTime",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClassStartTime",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorFullName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SessionNumber",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ClassDay",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ClassEndTime",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ClassStartTime",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProfessorFullName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SessionNumber",
                table: "OrderItems");

            migrationBuilder.AddColumn<double>(
                name: "PayAmount",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
