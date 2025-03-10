﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class EditOnCompnay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ComapnyAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ComapnyAccounts_UserId",
                table: "ComapnyAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComapnyAccounts_AspNetUsers_UserId",
                table: "ComapnyAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComapnyAccounts_AspNetUsers_UserId",
                table: "ComapnyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ComapnyAccounts_UserId",
                table: "ComapnyAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ComapnyAccounts");
        }
    }
}
