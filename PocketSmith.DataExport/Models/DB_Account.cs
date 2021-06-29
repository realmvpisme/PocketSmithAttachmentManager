using System;

namespace PocketSmith.DataExport.Models
{
    public class DB_Account : ModelBase<long>
    {
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public bool? IsNetWorth { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CurrentBalanceInBaseCurrency { get; set; }
        public string CurrentBalanceExchangeRate { get; set; }
        public DateTime CurrentBalanceDate { get; set; }
        public decimal? SafeBalance { get; set; }
        public decimal? SafeBalanceInBaseCurrency { get; set; }
        public decimal? StartingBalance { get; set; }
        public DateTime StartingBalanceDate { get; set; }

        public long? InstitutionId { get; set; }
        public DB_Institution Institution { get; set; }


    }
}
