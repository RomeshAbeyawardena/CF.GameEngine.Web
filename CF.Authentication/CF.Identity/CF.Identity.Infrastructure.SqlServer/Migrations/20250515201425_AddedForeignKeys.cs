using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessToken_Client_ClientId",
                schema: "dbo",
                table: "AccessToken");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_UserId",
                schema: "dbo",
                table: "AccessToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessToken_Client_ClientId",
                schema: "dbo",
                table: "AccessToken",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessToken_User_UserId",
                schema: "dbo",
                table: "AccessToken",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessToken_Client_ClientId",
                schema: "dbo",
                table: "AccessToken");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessToken_User_UserId",
                schema: "dbo",
                table: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_AccessToken_UserId",
                schema: "dbo",
                table: "AccessToken");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessToken_Client_ClientId",
                schema: "dbo",
                table: "AccessToken",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
