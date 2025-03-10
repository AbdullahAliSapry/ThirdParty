using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class AddPhysicalParametersToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotoalWheight",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalParameters",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhysicalParameters",
                table: "CartItems");

            migrationBuilder.AddColumn<double>(
                name: "TotoalWheight",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
