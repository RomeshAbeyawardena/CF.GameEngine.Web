using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.GameEngine.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class addedsortorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ElementType",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ElementType");
        }
    }
}
