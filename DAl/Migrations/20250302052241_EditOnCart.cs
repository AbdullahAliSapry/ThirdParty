using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class EditOnCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoldQuntity",
                table: "CartItem",
                newName: "Quntity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartItem",
                newName: "PricePerPiece");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "CartItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "AttributeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeItems_CartItem_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeItems_CartItemId",
                table: "AttributeItems",
                column: "CartItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeItems");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "Quntity",
                table: "CartItem",
                newName: "SoldQuntity");

            migrationBuilder.RenameColumn(
                name: "PricePerPiece",
                table: "CartItem",
                newName: "Price");
        }
    }
}
