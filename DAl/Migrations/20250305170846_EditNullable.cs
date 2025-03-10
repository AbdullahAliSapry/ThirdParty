using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class EditNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComapnyAccounts_FileUploads_FileId",
                table: "ComapnyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ComapnyAccounts_FileId",
                table: "ComapnyAccounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "ComapnyAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ComapnyAccounts_FileId",
                table: "ComapnyAccounts",
                column: "FileId",
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ComapnyAccounts_FileUploads_FileId",
                table: "ComapnyAccounts",
                column: "FileId",
                principalTable: "FileUploads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComapnyAccounts_FileUploads_FileId",
                table: "ComapnyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ComapnyAccounts_FileId",
                table: "ComapnyAccounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "ComapnyAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComapnyAccounts_FileId",
                table: "ComapnyAccounts",
                column: "FileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComapnyAccounts_FileUploads_FileId",
                table: "ComapnyAccounts",
                column: "FileId",
                principalTable: "FileUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
