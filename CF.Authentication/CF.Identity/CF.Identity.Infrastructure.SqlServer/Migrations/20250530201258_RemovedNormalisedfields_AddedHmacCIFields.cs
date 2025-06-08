using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNormalisedfields_AddedHmacCIFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_UserRole_DbUserRoleId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRole_UserId_RoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RoleScope_RoleId_ScopeId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropColumn(
                name: "ValueNormalised",
                schema: "dbo",
                table: "CommonName");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "dbo",
                table: "UserRole",
                newName: "AccessRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_RoleId",
                schema: "dbo",
                table: "UserRole",
                newName: "IX_UserRole_AccessRoleId");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "AccessRoleId");

            migrationBuilder.RenameColumn(
                name: "DbUserRoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "DbUserAccessRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScope_DbUserRoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "IX_RoleScope_DbUserAccessRoleId");

            migrationBuilder.AddColumn<Guid>(
                name: "DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "MetaData",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RowVersion",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValueCI",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValueHmac",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRole_UserId_AccessRoleId",
                schema: "dbo",
                table: "UserRole",
                columns: new[] { "UserId", "AccessRoleId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RoleScope_AccessRoleId_ScopeId",
                schema: "dbo",
                table: "RoleScope",
                columns: new[] { "AccessRoleId", "ScopeId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole",
                column: "DbUserAccessRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_AccessRoleId",
                schema: "dbo",
                table: "RoleScope",
                column: "AccessRoleId",
                principalSchema: "dbo",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "RoleScope",
                column: "DbUserAccessRoleId",
                principalSchema: "dbo",
                principalTable: "UserRole",
                principalColumn: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_AccessRoleId",
                schema: "dbo",
                table: "UserRole",
                column: "AccessRoleId",
                principalSchema: "dbo",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole",
                column: "DbUserAccessRoleId",
                principalSchema: "dbo",
                principalTable: "UserRole",
                principalColumn: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_AccessRoleId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_AccessRoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRole_UserId_AccessRoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RoleScope_AccessRoleId_ScopeId",
                schema: "dbo",
                table: "RoleScope");

            migrationBuilder.DropColumn(
                name: "DbUserAccessRoleId",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "MetaData",
                schema: "dbo",
                table: "CommonName");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "dbo",
                table: "CommonName");

            migrationBuilder.DropColumn(
                name: "ValueCI",
                schema: "dbo",
                table: "CommonName");

            migrationBuilder.DropColumn(
                name: "ValueHmac",
                schema: "dbo",
                table: "CommonName");

            migrationBuilder.RenameColumn(
                name: "AccessRoleId",
                schema: "dbo",
                table: "UserRole",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_AccessRoleId",
                schema: "dbo",
                table: "UserRole",
                newName: "IX_UserRole_RoleId");

            migrationBuilder.RenameColumn(
                name: "DbUserAccessRoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "DbUserRoleId");

            migrationBuilder.RenameColumn(
                name: "AccessRoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScope_DbUserAccessRoleId",
                schema: "dbo",
                table: "RoleScope",
                newName: "IX_RoleScope_DbUserRoleId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "ValueNormalised",
                schema: "dbo",
                table: "CommonName",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRole_UserId_RoleId",
                schema: "dbo",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RoleScope_RoleId_ScopeId",
                schema: "dbo",
                table: "RoleScope",
                columns: new[] { "RoleId", "ScopeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                schema: "dbo",
                table: "RoleScope",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_UserRole_DbUserRoleId",
                schema: "dbo",
                table: "RoleScope",
                column: "DbUserRoleId",
                principalSchema: "dbo",
                principalTable: "UserRole",
                principalColumn: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                schema: "dbo",
                table: "UserRole",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
