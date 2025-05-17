using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedHMAC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddressHmac",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsernameHmac",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddressHmac",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UsernameHmac",
                schema: "dbo",
                table: "User");
        }
    }
}
