using System;

namespace PocketSmith.DataExport.Models
{
    public class DB_BudgetEvent : ModelBase<string>, ISoftDeletable
    {
        public decimal? Amount { get; set; }
        public decimal? AmountInBaseCurrency { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? Date { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public string RepeatType { get; set; }
        public int RepeatInterval { get; set; }
        public long SeriesId { get; set; }
        public string SeriesStartId { get; set; }
        public bool? InfiniteSeries { get; set; }
        public bool Deleted { get; set; }

        public long CategoryId { get; set; }
        public DB_Category Category { get; set; }
        public long ScenarioId { get; set; }
        public DB_Scenario Scenario { get; set; }
    }
}