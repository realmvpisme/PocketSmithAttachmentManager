﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketSmith.DataExport.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    IsTransfer = table.Column<bool>(nullable: false),
                    IsBill = table.Column<bool>(nullable: false),
                    RefundBehaviour = table.Column<string>(nullable: true),
                    RollUp = table.Column<bool>(nullable: false),
                    ParentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeMetas",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeMetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Variants",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    ThumbUrl = table.Column<string>(nullable: true),
                    LargeUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsNetWorth = table.Column<bool>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrentBalance = table.Column<decimal>(nullable: false),
                    CurrentBalanceInBaseCurrency = table.Column<decimal>(nullable: false),
                    CurrentBalanceExchangeRate = table.Column<string>(nullable: true),
                    CurrentBalanceDate = table.Column<DateTime>(nullable: false),
                    SafeBalance = table.Column<decimal>(nullable: true),
                    SafeBalanceInBaseCurrency = table.Column<decimal>(nullable: true),
                    StartingBalance = table.Column<decimal>(nullable: true),
                    StartingBalanceDate = table.Column<DateTime>(nullable: false),
                    InstitutionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Payee = table.Column<string>(nullable: true),
                    OriginalPayee = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    UploadSource = table.Column<string>(nullable: true),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    CheckNumber = table.Column<int>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    AmountInBaseCurrency = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsTransfer = table.Column<bool>(nullable: true),
                    NeedsReview = table.Column<bool>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Labels = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    AccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    OriginalUrl = table.Column<string>(nullable: true),
                    VariantId = table.Column<long>(nullable: true),
                    VariantsId = table.Column<long>(nullable: true),
                    ContentTypeMetaId = table.Column<long>(nullable: true),
                    TransactionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_ContentTypeMetas_ContentTypeMetaId",
                        column: x => x.ContentTypeMetaId,
                        principalTable: "ContentTypeMetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Variants_VariantsId",
                        column: x => x.VariantsId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_InstitutionId",
                table: "Accounts",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ContentTypeMetaId",
                table: "Attachments",
                column: "ContentTypeMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TransactionId",
                table: "Attachments",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_VariantsId",
                table: "Attachments",
                column: "VariantsId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ContentTypeMetas");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Variants");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Institutions");
        }
    }
}
