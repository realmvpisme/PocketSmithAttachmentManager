using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketSmith.DataExport.Migrations
{
    public partial class AddBalanceSheetEntryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceSheetEntry",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    AccountBalance = table.Column<decimal>(nullable: false),
                    FirstOfMonthDate = table.Column<DateTime>(nullable: false),
                    TransactionId = table.Column<long>(nullable: true),
                    TransactionAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceSheetEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceSheetEntry_TransactionAccounts_TransactionAccountId",
                        column: x => x.TransactionAccountId,
                        principalTable: "TransactionAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BalanceSheetEntry_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheetEntry_TransactionAccountId",
                table: "BalanceSheetEntry",
                column: "TransactionAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheetEntry_TransactionId",
                table: "BalanceSheetEntry",
                column: "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceSheetEntry");
        }
    }
}
