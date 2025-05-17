using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookupFirstName",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LookupLastName",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LookupMiddleName",
                schema: "dbo",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LookupFirstName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LookupLastName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LookupMiddleName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
