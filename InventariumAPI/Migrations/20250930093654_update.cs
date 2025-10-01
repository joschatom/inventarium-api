using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventariumAPI.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lendouts_ObjectId",
                table: "Lendouts",
                column: "ObjectId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lendouts_ObjectId",
                table: "Lendouts");
        }
    }
}
