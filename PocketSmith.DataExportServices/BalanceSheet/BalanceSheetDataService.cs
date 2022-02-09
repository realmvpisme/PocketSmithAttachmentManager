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
            Console.Clear();
            ExtendedConsole.WriteInfo("Processing balance sheet entries...");
            var context = _contextFactory.Create(_databaseFilePath);

            var dbTransactions = await context.Transactions.ToListAsync();
            var dbBalanceSheetEntries = await context.BalanceSheetEntries.ToListAsync();



            List<DB_BalanceSheetEntry> balanceSheetTransactions =
                getAccountBalancesFromTransactions(dbTransactions, dbBalanceSheetEntries);

            foreach (var newEntry in balanceSheetTransactions)
            {
                if(dbBalanceSheetEntries.All(x => 
                       x.TransactionAccountId != newEntry.TransactionAccountId
                       && x.FirstOfMonthDate != newEntry.FirstOfMonthDate
                       && x.AccountBalance != newEntry.AccountBalance)
                   && newEntry.FirstOfMonthDate <= DateTime.Today)
                {
                    await context.BalanceSheetEntries.AddAsync(newEntry);
                }
            }

            var fixedAssetBalances = await getFixedAssetBalances(context, dbBalanceSheetEntries);

            foreach (var assetEntry in fixedAssetBalances)
            {
                var existingDatabaseEntry = dbBalanceSheetEntries.FirstOrDefault(x =>
                    x.TransactionAccountId == assetEntry.TransactionAccountId
                    && x.FirstOfMonthDate.ToUniversalTime() == assetEntry.FirstOfMonthDate);

                var existingTransactionEntry = balanceSheetTransactions.FirstOrDefault(x =>
                    x.TransactionAccountId == assetEntry.TransactionAccountId
                    && x.FirstOfMonthDate.ToUniversalTime() == assetEntry.FirstOfMonthDate);


                if (existingDatabaseEntry == null 
                    && existingTransactionEntry == null
                    && assetEntry.FirstOfMonthDate <= DateTime.Today)
                {
                    await context.BalanceSheetEntries.AddAsync(assetEntry);
                }
            }

            await context.SaveChangesAsync();

            ExtendedConsole.WriteSuccess("Balance sheet entries processed successfully!");
        }

        private List<DB_BalanceSheetEntry> getAccountBalancesFromTransactions(List<DB_Transaction> dbTransactions, List<DB_BalanceSheetEntry> dbBalanceSheetEntries)
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

            var transactionAccountIds = selectedTransactions.Select(x => x.AccountId.GetValueOrDefault()).Distinct().ToList();

            foreach (var accountId in transactionAccountIds)
            {
                var accountTransactions = selectedTransactions.Where(x => x.AccountId == accountId).ToList();

                var lastTransactionId = accountTransactions
                    .Max(x => x.Id);
                var lastTransaction = accountTransactions.First(x => x.Id == lastTransactionId);

                var maxBalanceSheetDate = DateTime.Parse(lastTransaction.Date).ToFirstOfNextMonth();

                foreach (var transaction in accountTransactions)
                {
                    var transactionDate = DateTime.Parse(transaction.Date);


                    var balanceSheetEntryDate = transactionDate.ToFirstOfNextMonth();

                        if (balanceSheetEntryDate <= maxBalanceSheetDate && balanceSheetEntryDate <= DateTime.Today)
                        {
                            var entry = new DB_BalanceSheetEntry
                            {
                                AccountBalance = transaction.ClosingBalance,
                                TransactionId = transaction.Id,
                                TransactionAccountId = transaction.AccountId.GetValueOrDefault(),
                                FirstOfMonthDate = balanceSheetEntryDate,
                                CreatedTime = DateTime.UtcNow,
                                LastUpdated = DateTime.UtcNow,
                                OriginalTransactionDate = DateTime.Parse(transaction.Date)
                            };

                            if (!balanceSheetEntryExists(entry, dbBalanceSheetEntries))
                            {
                                balanceSheetEntries.Add(entry);
                            }
                        }
                }

            }

            balanceSheetEntries = balanceSheetEntries.FillMissingBalanceSheetEntries();

            return balanceSheetEntries;
        }

        private async Task<List<DB_BalanceSheetEntry>> getFixedAssetBalances(PocketSmithDbContext context, List<DB_BalanceSheetEntry> dbBalanceSheetEntries)
        {
            var resultSheetEntries = new List<DB_BalanceSheetEntry>();

            var fixedAssets = await context.Accounts
                .Include(x => x.TransactionAccounts)
                .Include(x => x.AccountBalances)
                .Where(x => x.Type == "vehicle" || x.Type == "property" || x.Type == "other_asset").ToListAsync();

            foreach (var asset in fixedAssets)
            {
                var assetBalances = getLatestAssetBalances(asset);

                assetBalances.ForEach(x => resultSheetEntries.Add(x.ToBalanceSheetEntry()));
            }

            resultSheetEntries = resultSheetEntries.FillMissingBalanceSheetEntries();

            return resultSheetEntries;
        }

        private List<DB_AccountBalance> getLatestAssetBalances(DB_Account asset)
        {
            var resultAccountBalances = new List<DB_AccountBalance>();

            var firstBalance = asset.AccountBalances.OrderBy(x => x.Date).FirstOrDefault();
            var lastBalance = asset.AccountBalances.OrderBy(x => x.Date).LastOrDefault();

            if (firstBalance == null && lastBalance == null)
            {
                return resultAccountBalances;
            }

            var comparer = new ObjectsComparer.Comparer<DB_AccountBalance>();
            comparer.IgnoreMember("CreatedTime");
            comparer.IgnoreMember("LastUpdated");
            comparer.IgnoreMember("Id");

            var differences = comparer.CalculateDifferences(firstBalance, lastBalance);

            if (!differences.Any())
            { 
                resultAccountBalances.Add(firstBalance);
                return resultAccountBalances;
            }

            var latestMonthlyBalances = getLatestAccountBalances(asset.AccountBalances);

            resultAccountBalances.AddRange(latestMonthlyBalances);
            return resultAccountBalances;
        }
        private List<DB_AccountBalance> getLatestAccountBalances(IEnumerable<DB_AccountBalance> inputBalances)
        {
            var resultBalances = new List<DB_AccountBalance>();
            var localInputBalances = inputBalances.ToList();
            foreach (var balance in localInputBalances)
            {
                var greatestMonthlyBalances = localInputBalances.Where(x =>
                    x.Date.Year == balance.Date.Year
                    && x.Date.Month == balance.Date.Month).ToList();
                var maxDate = greatestMonthlyBalances.Max(x => x.Date);
                var greatestBalance = greatestMonthlyBalances.FirstOrDefault(x => x.Date == maxDate);
                if (greatestBalance == balance)
                {
                    resultBalances.Add(balance);
                }
            }

            return resultBalances;
        }

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
            while(currentDate < endDate)
            {
                currentDate = new DateTime(currentDate.Month < 12 ? currentDate.Year : currentDate.Year + 1,
                        currentDate.Month < 12 ? currentDate.Month + 1 : 1, 1);
                if (currentDate < endDate)
                {
                    interimDates.Add(currentDate);
                }
            }

            return interimDates;
        }
    }
}