using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.GameEngine.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Element_ElementTypeId",
                table: "Element",
                column: "ElementTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Element_ElementType_ElementTypeId",
                table: "Element",
                column: "ElementTypeId",
                principalTable: "ElementType",
                principalColumn: "ElementTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Element_ElementType_ElementTypeId",
                table: "Element");

            migrationBuilder.DropIndex(
                name: "IX_Element_ElementTypeId",
                table: "Element");
        }
    }
}
