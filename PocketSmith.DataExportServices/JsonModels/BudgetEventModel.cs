using System;
using System.Text.Json.Serialization;

namespace PocketSmith.DataExportServices.JsonModels
{
    public class BudgetEventModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("category")]
        public CategoryModel Category { get; set; }
        [JsonPropertyName("scenario")]
        public ScenarioModel Scenario { get; set; }
        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }
        [JsonPropertyName("amount_in_base_currency")]
        public decimal? AmountInBaseCurrency { get; set; }
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
        [JsonPropertyName("colour")]
        public string Color { get; set; }
        [JsonPropertyName("note")]
        public string Note { get; set; }
        [JsonPropertyName("repeat_type")]
        public string RepeatType { get; set; }
        [JsonPropertyName("repeat_interval")]
        public int RepeatInterval { get; set; }
        [JsonPropertyName("series_id")]
        public long SeriesId { get; set; }
        [JsonPropertyName("series_start_id")]
        public string SeriesStartId { get; set; }
        [JsonPropertyName("infinite_series")]
        public bool? InfiniteSeries { get; set; }
    }
}