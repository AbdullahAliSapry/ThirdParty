using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAl.Migrations
{
    /// <inheritdoc />
    public partial class NewBillingMArketer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillingToMarketrs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarkterAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingToMarketrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingToMarketrs_FileUploads_FileId",
                        column: x => x.FileId,
                        principalTable: "FileUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillingToMarketrs_MarketerAccounts_MarkterAccountId",
                        column: x => x.MarkterAccountId,
                        principalTable: "MarketerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillingToMarketrs_FileId",
                table: "BillingToMarketrs",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingToMarketrs_MarkterAccountId",
                table: "BillingToMarketrs",
                column: "MarkterAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingToMarketrs");
        }
    }
}
