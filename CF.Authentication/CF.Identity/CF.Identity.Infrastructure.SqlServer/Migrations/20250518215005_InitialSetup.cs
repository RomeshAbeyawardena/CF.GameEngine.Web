using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "dbo",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuspendedTimestampUtc = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ValidFrom = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: true),
                    SecretHash = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

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

            migrationBuilder.CreateTable(
                name: "Scope",
                schema: "dbo",
                columns: table => new
                {
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scope", x => x.ScopeId);
                    table.ForeignKey(
                        name: "FK_Scope_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EmailAddressHmac = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    UsernameHmac = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    FirstCommonNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MiddleCommonNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastCommonNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredUsername = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PreferredUsernameHmac = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PrimaryTelephoneNumber = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PrimaryTelephoneNumberHmac = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_CommonName_FirstCommonNameId",
                        column: x => x.FirstCommonNameId,
                        principalSchema: "dbo",
                        principalTable: "CommonName",
                        principalColumn: "CommonNameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_CommonName_LastCommonNameId",
                        column: x => x.LastCommonNameId,
                        principalSchema: "dbo",
                        principalTable: "CommonName",
                        principalColumn: "CommonNameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_CommonName_MiddleCommonNameId",
                        column: x => x.MiddleCommonNameId,
                        principalSchema: "dbo",
                        principalTable: "CommonName",
                        principalColumn: "CommonNameId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AccessToken",
                schema: "dbo",
                columns: table => new
                {
                    AccessTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceToken = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: true),
                    SuspendedTimestampUtc = table.Column<DateTimeOffset>(type: "datetimeoffset(7)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessToken", x => x.AccessTokenId);
                    table.ForeignKey(
                        name: "FK_AccessToken_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserScope",
                schema: "dbo",
                columns: table => new
                {
                    UserScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScope", x => x.UserScopeId);
                    table.UniqueConstraint("AK_UserScope_UserId_ScopeId", x => new { x.UserId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_UserScope_Scope_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "dbo",
                        principalTable: "Scope",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserScope_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_ClientId",
                schema: "dbo",
                table: "AccessToken",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_UserId",
                schema: "dbo",
                table: "AccessToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scope_ClientId",
                schema: "dbo",
                table: "Scope",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ClientId",
                schema: "dbo",
                table: "User",
                column: "ClientId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserScope_ScopeId",
                schema: "dbo",
                table: "UserScope",
                column: "ScopeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserScope",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Scope",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CommonName",
                schema: "dbo");
        }
    }
}
