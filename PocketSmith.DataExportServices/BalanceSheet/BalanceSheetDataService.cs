using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Extensions;
using PocketSmith.DataExport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketSmith.DataExportServices.BalanceSheet
{
    public class BalanceSheetDataService
    {
        private readonly string _databaseFilePath;
        private readonly ContextFactory _contextFactory;

        public BalanceSheetDataService(string databaseFilePath)
        {
            _databaseFilePath = databaseFilePath;
            _contextFactory = new ContextFactory();
        }
        public async Task UpdateBalanceSheetEntries()
        {
            ExtendedConsole.WriteInfo("Processing balance sheet entries...");
            var context = _contextFactory.Create(_databaseFilePath);

            var dbTransactions = await context.Transactions.ToListAsync();
            var dbBalanceSheetEntries = await context.BalanceSheetEntries.ToListAsync();



            List<DB_BalanceSheetEntry> balanceSheetTransactions =
                getAccountBalances(dbTransactions, dbBalanceSheetEntries);

            await context.BalanceSheetEntries.AddRangeAsync(balanceSheetTransactions);
            await context.SaveChangesAsync();

            ExtendedConsole.WriteSuccess("Balance sheet entries processed successfully!");
        }

        private List<DB_BalanceSheetEntry> getAccountBalances(List<DB_Transaction> dbTransactions, List<DB_BalanceSheetEntry> dbBalanceSheetEntries)
        {
            var balanceSheetEntries = new List<DB_BalanceSheetEntry>();

            List<DB_Transaction> selectedTransactions = new List<DB_Transaction>();
            //If an account has more than one transaction in a month, we want the balance of the last transaction for the month.
            foreach (var transaction in dbTransactions)
            {
                if (dbTransactions.Any(x =>
                    x.AccountId == transaction.AccountId
                    && DateTime.Parse(x.Date).Year == DateTime.Parse(transaction.Date).Year
                    && DateTime.Parse(x.Date).Month == DateTime.Parse(transaction.Date).Month
                    && x.Id > transaction.Id))
                {
                    continue;
                }

                selectedTransactions.Add(transaction);
            }

            var transactionAccountIds = selectedTransactions.Select(x => x.AccountId.GetValueOrDefault()).Distinct();

            foreach (var accountId in transactionAccountIds)
            {
                var firstTransactionId = selectedTransactions
                    .Min(x => x.Id);

                var lastTransactionId = selectedTransactions
                    .Max(x => x.Id);
                var lastTransaction = selectedTransactions.First(x => x.Id == lastTransactionId);

                var lastTransactionDate = DateTime.Parse(lastTransaction.Date);

                var maxBalanceSheetDate = new DateTime(
                    lastTransactionDate.Month < 12 ? lastTransactionDate.Year : lastTransactionDate.Year + 1,
                    lastTransactionDate.Month < 12 ? lastTransactionDate.Month : lastTransactionDate.Month + 1, 1);


                var accountTransactions = selectedTransactions.Where(x => x.AccountId == accountId);


                foreach (var transaction in accountTransactions)
                {
                    var transactionDate = DateTime.Parse(transaction.Date);

                   
                        var balanceSheetEntryDate = new DateTime(
                            transactionDate.Month < 12 ? transactionDate.Year : transactionDate.Year + 1,
                            transactionDate.Month < 12 ? transactionDate.Month + 1 : 1,
                            1);

                        if (balanceSheetEntryDate <= maxBalanceSheetDate && balanceSheetEntryDate <= DateTime.Today)
                        {
                            var entry = new DB_BalanceSheetEntry
                            {
                                AccountBalance = transaction.ClosingBalance,
                                TransactionId = transaction.Id,
                                TransactionAccountId = transaction.AccountId.GetValueOrDefault(),
                                FirstOfMonthDate = balanceSheetEntryDate,
                                CreatedTime = DateTime.UtcNow,
                                LastUpdated = DateTime.UtcNow
                            };

                            if (!balanceSheetEntryExists(entry, dbBalanceSheetEntries))
                            {
                                balanceSheetEntries.Add(entry);
                            }
                        }
                }

                //Verify all entries between the first and last exist. Add them if they don't.
                var selectedAccountBalanceEntries = balanceSheetEntries.Where(x => x.TransactionAccountId == accountId);
                if (!selectedAccountBalanceEntries.Any())
                {
                    continue;
                }
                var oldestBalanceDate = selectedAccountBalanceEntries.Min(x => x.FirstOfMonthDate);

                var newestBalanceDate = selectedAccountBalanceEntries.Max(x => x.FirstOfMonthDate);

                var interimDates = getInterimFirstOfMonthDates(oldestBalanceDate, newestBalanceDate);

                foreach (var interimDate in interimDates)
                {
                    if (selectedAccountBalanceEntries.Select(x => x.FirstOfMonthDate).All(x => x != interimDate))
                    {
                        var maxAvailableDate = selectedAccountBalanceEntries
                            .Where(x => x.FirstOfMonthDate <= interimDate).Max(x => x.FirstOfMonthDate);
                        var newBalanceEntry = new DB_BalanceSheetEntry
                        {
                            AccountBalance = selectedAccountBalanceEntries
                                .FirstOrDefault(x => x.FirstOfMonthDate == maxAvailableDate).AccountBalance,
                            FirstOfMonthDate = interimDate,
                            TransactionAccountId = accountId,
                            CreatedTime = DateTime.UtcNow,
                            LastUpdated = DateTime.UtcNow
                        };

                        if (!balanceSheetEntryExists(newBalanceEntry, dbBalanceSheetEntries))
                        {
                            balanceSheetEntries.Add(newBalanceEntry);
                        }
                    }
                }

            }

            return balanceSheetEntries;
        }

        //private async Task<List<DB_BalanceSheetEntry>> getFixedAssetBalances(PocketSmithDbContext context)
        //{
        //    var fixedAssets = await context.TransactionAccounts
        //        .Where(x => x.Type == "vehicles" || x.Type == "property" || x.Type == "other_asset").ToListAsync();

        //    foreach (var asset in fixedAssets)
        //    {
                
        //    }
        //}

        private bool balanceSheetEntryExists(DB_BalanceSheetEntry entry, List<DB_BalanceSheetEntry> dbBalanceSheetEntries)
        {
            return dbBalanceSheetEntries.Any(x =>
                x.FirstOfMonthDate == entry.FirstOfMonthDate && x.TransactionAccountId == entry.TransactionAccountId &&
                x.AccountBalance == entry.AccountBalance);
        }

        private List<DateTime> getInterimFirstOfMonthDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> interimDates = new List<DateTime>();

            var currentDate = startDate;
            do
            {
                currentDate = new DateTime(currentDate.Month < 12 ? currentDate.Year : currentDate.Year + 1,
                        currentDate.Month < 12 ? currentDate.Month + 1 : 1, 1);
                    interimDates.Add(currentDate);

            } while (currentDate < endDate);

            return interimDates;
        }
    }
}