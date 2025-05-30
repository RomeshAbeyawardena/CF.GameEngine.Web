using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ColumnMods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValueCI",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(344)",
                maxLength: 344,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValueCI",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(344)",
                oldMaxLength: 344);
        }
    }
}
