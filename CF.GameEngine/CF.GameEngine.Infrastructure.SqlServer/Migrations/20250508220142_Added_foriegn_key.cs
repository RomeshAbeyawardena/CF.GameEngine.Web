using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CF.GameEngine.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Added_foriegn_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Element_ParentElementId",
                table: "Element",
                column: "ParentElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Element_Element_ParentElementId",
                table: "Element",
                column: "ParentElementId",
                principalTable: "Element",
                principalColumn: "ElementId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Element_Element_ParentElementId",
                table: "Element");

            migrationBuilder.DropIndex(
                name: "IX_Element_ParentElementId",
                table: "Element");
        }
    }
}
