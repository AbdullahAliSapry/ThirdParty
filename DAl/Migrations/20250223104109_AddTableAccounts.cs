using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketerAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameOfBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfmethode = table.Column<int>(type: "int", nullable: false),
                    NameOfAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    MarketerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketerAccounts_Marketers_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "Marketers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketerAccounts_MarketerId",
                table: "MarketerAccounts",
                column: "MarketerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketerAccounts");
        }
    }
}
