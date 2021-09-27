using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketSmith.DataExport.Migrations
{
    public partial class AddSoftDeleteColumnToAccountTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TransactionAccounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TransactionAccounts");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Accounts");
        }
    }
}
