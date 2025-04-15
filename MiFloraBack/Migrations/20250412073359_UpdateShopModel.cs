using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiFloraBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Branches_BranchId",
                table: "Shops");

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                table: "Shops",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Shops",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OwnerId",
                table: "Shops",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Branches_BranchId",
                table: "Shops",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Users_OwnerId",
                table: "Shops",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Branches_BranchId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Users_OwnerId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_OwnerId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Shops");

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                table: "Shops",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Branches_BranchId",
                table: "Shops",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
