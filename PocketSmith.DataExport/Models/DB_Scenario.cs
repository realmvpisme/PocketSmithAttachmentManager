using System;
using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Scenario : ModelBase
    {
        public long AccountId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? InterestRate { get; set; }
        public long? InterestRateRepeatId { get; set; }
        public string Type { get; set; }
        public bool? IsNetWorth { get; set; }
        public decimal? MinimumValue { get; set; }
        public decimal? MaximumValue { get; set; }
        public DateTime? AchieveDate { get; set; }
        public decimal? StartingBalance { get; set; }
        public DateTime StartingBalanceDate { get; set; }
        public decimal? ClosingBalance { get; set; }
        public DateTime? ClosingBalanceDate { get; set; }
        public decimal? CurrentBalance { get; set; }
        public decimal? CurrentBalanceInBaseCurrency { get; set; }
        public string CurrentBalanceExchangeRate { get; set; }
        public DateTime? CurrentBalanceDate { get; set; }
        public decimal? SafeBalance { get; set; }
        public decimal? SafeBalanceInBaseCurrency { get; set; }

        public ICollection<DB_BudgetEvent> BudgetEvents { get; set; }
    }
}