using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserCaseImpressions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddressCI",
                schema: "dbo",
                table: "User",
                type: "nvarchar(344)",
                maxLength: 344,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredUsernameCI",
                schema: "dbo",
                table: "User",
                type: "nvarchar(344)",
                maxLength: 344,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsernameCI",
                schema: "dbo",
                table: "User",
                type: "nvarchar(344)",
                maxLength: 344,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddressCI",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PreferredUsernameCI",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UsernameCI",
                schema: "dbo",
                table: "User");
        }
    }
}
