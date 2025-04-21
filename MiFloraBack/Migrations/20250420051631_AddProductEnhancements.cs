using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiFloraBack.Migrations
{
    /// <inheritdoc />
    public partial class AddProductEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Businesses_BusinessId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Categories",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_BusinessId",
                table: "Categories",
                newName: "IX_Categories_ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Shops_ShopId",
                table: "Categories",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Shops_ShopId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "Categories",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ShopId",
                table: "Categories",
                newName: "IX_Categories_BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Businesses_BusinessId",
                table: "Categories",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
