using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.GameEngine.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementType",
                columns: table => new
                {
                    ElementTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalReference = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementType", x => x.ElementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Element",
                columns: table => new
                {
                    ElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalReference = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    ElementTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Element", x => x.ElementId);
                    table.ForeignKey(
                        name: "FK_Element_ElementType_ElementTypeId",
                        column: x => x.ElementTypeId,
                        principalTable: "ElementType",
                        principalColumn: "ElementTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Element_ElementTypeId",
                table: "Element",
                column: "ElementTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Element");

            migrationBuilder.DropTable(
                name: "ElementType");
        }
    }
}
