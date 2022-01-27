using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketSmith.DataExport.Migrations
{
    public partial class RemovedUniqueConstraintOnTransactionAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BalanceSheetEntry_TransactionAccountId",
                table: "BalanceSheetEntry");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheetEntry_TransactionAccountId",
                table: "BalanceSheetEntry",
                column: "TransactionAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BalanceSheetEntry_TransactionAccountId",
                table: "BalanceSheetEntry");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheetEntry_TransactionAccountId",
                table: "BalanceSheetEntry",
                column: "TransactionAccountId",
                unique: true);
        }
    }
}
