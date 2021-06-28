using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketSmith.DataExport.Migrations
{
    public partial class AddBudgetEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InterestRate = table.Column<decimal>(nullable: true),
                    InterestRateRepeatId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsNetWorth = table.Column<bool>(nullable: true),
                    MinimumValue = table.Column<decimal>(nullable: true),
                    MaximumValue = table.Column<decimal>(nullable: true),
                    AchieveDate = table.Column<DateTime>(nullable: true),
                    StartingBalance = table.Column<decimal>(nullable: true),
                    StartingBalanceDate = table.Column<DateTime>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: true),
                    ClosingBalanceDate = table.Column<DateTime>(nullable: true),
                    CurrentBalance = table.Column<decimal>(nullable: true),
                    CurrentBalanceInBaseCurrency = table.Column<decimal>(nullable: true),
                    CurrentBalanceExchangeRate = table.Column<string>(nullable: true),
                    CurrentBalanceDate = table.Column<DateTime>(nullable: true),
                    SafeBalance = table.Column<decimal>(nullable: true),
                    SafeBalanceInBaseCurrency = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetEvents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: true),
                    AmountInBaseCurrency = table.Column<decimal>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    RepeatType = table.Column<string>(nullable: true),
                    RepeatInterval = table.Column<int>(nullable: false),
                    SeriesId = table.Column<long>(nullable: false),
                    SeriesStartId = table.Column<string>(nullable: true),
                    InfiniteSeries = table.Column<bool>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    ScenarioId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetEvents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetEvents_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetEvents_CategoryId",
                table: "BudgetEvents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetEvents_ScenarioId",
                table: "BudgetEvents",
                column: "ScenarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetEvents");

            migrationBuilder.DropTable(
                name: "Scenarios");
        }
    }
}
