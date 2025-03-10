using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class AccountTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "PayMentManouls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NameAccountTransferFrom",
                table: "PayMentManouls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumberOfAccountTransferFrom",
                table: "PayMentManouls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameofAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOfBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayMentManouls_AccountId",
                table: "PayMentManouls",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayMentManouls_Accounts_AccountId",
                table: "PayMentManouls",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayMentManouls_Accounts_AccountId",
                table: "PayMentManouls");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_PayMentManouls_AccountId",
                table: "PayMentManouls");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PayMentManouls");

            migrationBuilder.DropColumn(
                name: "NameAccountTransferFrom",
                table: "PayMentManouls");

            migrationBuilder.DropColumn(
                name: "NumberOfAccountTransferFrom",
                table: "PayMentManouls");
        }
    }
}
