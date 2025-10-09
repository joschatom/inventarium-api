using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventariumAPI.Migrations
{
    /// <inheritdoc />
    public partial class broken2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrokenObjectObjectId",
                table: "Objects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_BrokenObjectObjectId",
                table: "Objects",
                column: "BrokenObjectObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_BrokenObjects_BrokenObjectObjectId",
                table: "Objects",
                column: "BrokenObjectObjectId",
                principalTable: "BrokenObjects",
                principalColumn: "ObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_BrokenObjects_BrokenObjectObjectId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_BrokenObjectObjectId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "BrokenObjectObjectId",
                table: "Objects");
        }
    }
}
