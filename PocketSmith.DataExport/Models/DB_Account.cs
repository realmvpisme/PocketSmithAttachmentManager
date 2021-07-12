using System;
using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Account : ModelBase<long>
    {
        public string Title { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CurrentBalanceInBaseCurrency { get; set; }
        public decimal? CurrentBalanceExchangeRate { get; set; }
        public DateTime? CurrentBalanceDate { get; set; }
        public decimal? SafeBalance { get; set; }
        public decimal? SafeBalanceInBaseCurrency { get; set; }
        public string Type { get; set; }
        public string IsNetWorth { get; set; }

        public long? PrimaryTransactionAccountId { get; set; }
        public DB_TransactionAccount PrimaryTransactionAccount { get; set; }
        public long? PrimaryScenarioId { get; set; }
        public DB_Scenario PrimaryScenario { get; set; }
        public ICollection<DB_TransactionAccount> TransactionAccounts { get; set; }
        public ICollection<DB_Scenario> Scenarios { get; set; }
        public ICollection<DB_AccountBalance> AccountBalances { get; set; }

    }
}