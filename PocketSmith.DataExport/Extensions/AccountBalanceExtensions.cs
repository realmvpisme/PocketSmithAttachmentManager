using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PocketSmith.DataExport.Models;

namespace PocketSmith.DataExport.Extensions
{
    public static class AccountBalanceExtensions
    {

        public static List<DB_BalanceSheetEntry> FillMissingBalanceSheetEntries(
            this IEnumerable<DB_BalanceSheetEntry> inputEntries)
        {
            var localEntries = inputEntries.ToList();
            if (localEntries.Count < 2)
            {
                throw new InvalidOperationException(
                    $"Invalid enumeration count. Parameter inputEntries requires at least two items to evaluate.");
            }

            var resultEntries = new List<DB_BalanceSheetEntry>();

            var accountIds = inputEntries.Select(x => x.TransactionAccountId).Distinct().ToList();

            foreach (var accountId in accountIds)
            {
                var selectedEntries = inputEntries.Where(x => x.TransactionAccountId == accountId);
                var firstEntry = selectedEntries.OrderBy(x => x.FirstOfMonthDate).FirstOrDefault();
                var lastEntry = selectedEntries.OrderBy(x => x.FirstOfMonthDate).LastOrDefault();
                var interimDates = getInterimFirstOfMonthDates(firstEntry.FirstOfMonthDate, lastEntry.FirstOfMonthDate);

                var interimEntries = new List<DB_BalanceSheetEntry>();
                foreach (var interimDate in interimDates)
                {
                    if (localEntries.Select(x => x.FirstOfMonthDate).Contains(interimDate))
                    {
                        continue;
                    }
                    var latestEntry = localEntries.LastOrDefault(x => x.FirstOfMonthDate <= interimDate);

                    var newEntry = new DB_BalanceSheetEntry
                    {
                        AccountBalance = latestEntry.AccountBalance,
                        CreatedTime = DateTime.UtcNow,
                        FirstOfMonthDate = interimDate,
                        LastUpdated = DateTime.UtcNow,
                        TransactionAccountId = latestEntry.TransactionAccountId
                    };

                    interimEntries.Add(newEntry);
                }
                resultEntries.AddRange(localEntries);
                resultEntries.AddRange(interimEntries);
            }

            return resultEntries;
        }

        public static DB_BalanceSheetEntry ToBalanceSheetEntry(this DB_AccountBalance inputAccountBalance)
        {
            var result = new DB_BalanceSheetEntry
            {
                AccountBalance = inputAccountBalance.Balance,
                CreatedTime = DateTime.UtcNow,
                FirstOfMonthDate = inputAccountBalance.Date.ToFirstOfNextMonth(),
                LastUpdated = DateTime.UtcNow,
                OriginalTransactionDate = inputAccountBalance.Date,
                TransactionAccountId = (long)inputAccountBalance.Account.PrimaryTransactionAccountId
            };

            return result;
        }

        private static List<DateTime> getInterimFirstOfMonthDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> interimDates = new List<DateTime>();

            var currentDate = startDate;
            while (currentDate < endDate)
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