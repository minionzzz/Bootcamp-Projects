using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendingWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Categories",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Categories");
        }
    }
}
