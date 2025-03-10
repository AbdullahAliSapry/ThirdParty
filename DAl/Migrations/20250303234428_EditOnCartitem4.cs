using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class EditOnCartitem4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "categoryId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_categoryId",
                table: "CartItems",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Categories_categoryId",
                table: "CartItems",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Categories_categoryId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_categoryId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "CartItems");
        }
    }
}
