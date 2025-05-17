using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommonName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstCommonNameId",
                schema: "dbo",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LastCommonNameId",
                schema: "dbo",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddColumn<Guid>(
                name: "MiddleCommonNameId",
                schema: "dbo",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CommonName",
                schema: "dbo",
                columns: table => new
                {
                    CommonNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ValueNormalised = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonName", x => x.CommonNameId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_FirstCommonNameId",
                schema: "dbo",
                table: "User",
                column: "FirstCommonNameId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastCommonNameId",
                schema: "dbo",
                table: "User",
                column: "LastCommonNameId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MiddleCommonNameId",
                schema: "dbo",
                table: "User",
                column: "MiddleCommonNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_CommonName_FirstCommonNameId",
                schema: "dbo",
                table: "User",
                column: "FirstCommonNameId",
                principalSchema: "dbo",
                principalTable: "CommonName",
                principalColumn: "CommonNameId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_CommonName_LastCommonNameId",
                schema: "dbo",
                table: "User",
                column: "LastCommonNameId",
                principalSchema: "dbo",
                principalTable: "CommonName",
                principalColumn: "CommonNameId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_CommonName_MiddleCommonNameId",
                schema: "dbo",
                table: "User",
                column: "MiddleCommonNameId",
                principalSchema: "dbo",
                principalTable: "CommonName",
                principalColumn: "CommonNameId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_CommonName_FirstCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_CommonName_LastCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_CommonName_MiddleCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropTable(
                name: "CommonName",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_User_FirstCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_LastCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_MiddleCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FirstCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastCommonNameId",
                schema: "dbo",
                table: "User");

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

            migrationBuilder.DropColumn(
                name: "MiddleCommonNameId",
                schema: "dbo",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                schema: "dbo",
                table: "User",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
