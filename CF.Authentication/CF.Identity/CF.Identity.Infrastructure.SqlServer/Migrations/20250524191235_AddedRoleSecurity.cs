using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoleSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivileged",
                schema: "dbo",
                table: "Scope",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "dbo",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Role_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                    table.UniqueConstraint("AK_UserRole_UserId_RoleId", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleScope",
                schema: "dbo",
                columns: table => new
                {
                    RoleScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DbUserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleScope", x => x.RoleScopeId);
                    table.UniqueConstraint("AK_RoleScope_RoleId_ScopeId", x => new { x.RoleId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_RoleScope_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleScope_Scope_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "dbo",
                        principalTable: "Scope",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleScope_UserRole_DbUserRoleId",
                        column: x => x.DbUserRoleId,
                        principalSchema: "dbo",
                        principalTable: "UserRole",
                        principalColumn: "UserRoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_ClientId_Key",
                schema: "dbo",
                table: "Role",
                columns: new[] { "ClientId", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_DbUserRoleId",
                schema: "dbo",
                table: "RoleScope",
                column: "DbUserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_ScopeId",
                schema: "dbo",
                table: "RoleScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "dbo",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleScope",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IsPrivileged",
                schema: "dbo",
                table: "Scope");
        }
    }
}
