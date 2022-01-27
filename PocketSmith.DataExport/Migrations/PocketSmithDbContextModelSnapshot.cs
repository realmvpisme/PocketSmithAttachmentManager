﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PocketSmith.DataExport;

namespace PocketSmith.DataExport.Migrations
{
    [DbContext(typeof(PocketSmithDbContext))]
    partial class PocketSmithDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.16");

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Account", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CurrentBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CurrentBalanceExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IsNetWorth")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<long?>("PrimaryScenarioId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("PrimaryTransactionAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("SafeBalance")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SafeBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryScenarioId")
                        .IsUnique();

                    b.HasIndex("PrimaryTransactionAccountId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_AccountBalance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountBalances");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Attachment", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContentType")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ContentTypeMetaId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TransactionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<long?>("VariantId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("VariantsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContentTypeMetaId");

                    b.HasIndex("TransactionId");

                    b.HasIndex("VariantsId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_BalanceSheetEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FirstOfMonthDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("OriginalTransactionDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("TransactionAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TransactionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TransactionAccountId");

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    b.ToTable("BalanceSheetEntry");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_BudgetEvent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("AmountInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<long>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("InfiniteSeries")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int>("RepeatInterval")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RepeatType")
                        .HasColumnType("TEXT");

                    b.Property<long>("ScenarioId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SeriesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SeriesStartId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ScenarioId");

                    b.ToTable("BudgetEvents");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Category", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBill")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTransfer")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RefundBehaviour")
                        .HasColumnType("TEXT");

                    b.Property<bool>("RollUp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_ContentTypeMeta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Extension")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ContentTypeMetas");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Institution", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Institutions");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Scenario", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("AchieveDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("ClosingBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ClosingBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CurrentBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CurrentBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentBalanceExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CurrentBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<long?>("InterestRateRepeatId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsNetWorth")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("MaximumValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("MinimumValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SafeBalance")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SafeBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("StartingBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartingBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Scenarios");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Transaction", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AmountInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<long?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CheckNumber")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ClosingBalance")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsTransfer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Labels")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Memo")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("NeedsReview")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalPayee")
                        .HasColumnType("TEXT");

                    b.Property<string>("Payee")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UploadSource")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_TransactionAccount", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CurrentBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentBalanceExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DB_AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("InstitutionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsNetWorth")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SafeBalance")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SafeBalanceInBaseCurrency")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("StartingBalance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartingBalanceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("DB_AccountId");

                    b.HasIndex("InstitutionId");

                    b.ToTable("TransactionAccounts");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Variant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LargeUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Account", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Scenario", "PrimaryScenario")
                        .WithOne()
                        .HasForeignKey("PocketSmith.DataExport.Models.DB_Account", "PrimaryScenarioId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PocketSmith.DataExport.Models.DB_TransactionAccount", "PrimaryTransactionAccount")
                        .WithOne()
                        .HasForeignKey("PocketSmith.DataExport.Models.DB_Account", "PrimaryTransactionAccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_AccountBalance", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Account", "Account")
                        .WithMany("AccountBalances")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Attachment", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_ContentTypeMeta", "ContentTypeMeta")
                        .WithMany("Attachments")
                        .HasForeignKey("ContentTypeMetaId");

                    b.HasOne("PocketSmith.DataExport.Models.DB_Transaction", "Transaction")
                        .WithMany("Attachments")
                        .HasForeignKey("TransactionId");

                    b.HasOne("PocketSmith.DataExport.Models.DB_Variant", "Variants")
                        .WithMany("Attachments")
                        .HasForeignKey("VariantsId");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_BalanceSheetEntry", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_TransactionAccount", "TransactionAccount")
                        .WithMany()
                        .HasForeignKey("TransactionAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PocketSmith.DataExport.Models.DB_Transaction", "Transaction")
                        .WithOne()
                        .HasForeignKey("PocketSmith.DataExport.Models.DB_BalanceSheetEntry", "TransactionId")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_BudgetEvent", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Category", "Category")
                        .WithMany("BudgetEvents")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PocketSmith.DataExport.Models.DB_Scenario", "Scenario")
                        .WithMany("BudgetEvents")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Category", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Scenario", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Account", "Account")
                        .WithMany("Scenarios")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_Transaction", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_TransactionAccount", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("PocketSmith.DataExport.Models.DB_Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("PocketSmith.DataExport.Models.DB_TransactionAccount", b =>
                {
                    b.HasOne("PocketSmith.DataExport.Models.DB_Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PocketSmith.DataExport.Models.DB_Account", null)
                        .WithMany("TransactionAccounts")
                        .HasForeignKey("DB_AccountId");

                    b.HasOne("PocketSmith.DataExport.Models.DB_Institution", "Institution")
                        .WithMany("TransactionAccounts")
                        .HasForeignKey("InstitutionId");
                });
#pragma warning restore 612, 618
        }
    }
}
