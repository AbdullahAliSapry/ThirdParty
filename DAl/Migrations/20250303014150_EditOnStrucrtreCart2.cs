using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class EditOnStrucrtreCart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AttributeItems");

            migrationBuilder.DropColumn(
                name: "Pid",
                table: "AttributeItems");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AttributeItems");

            migrationBuilder.RenameColumn(
                name: "Vid",
                table: "AttributeItems",
                newName: "AttributesJson");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AttributeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AttributeItems");

            migrationBuilder.RenameColumn(
                name: "AttributesJson",
                table: "AttributeItems",
                newName: "Vid");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AttributeItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pid",
                table: "AttributeItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "AttributeItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
