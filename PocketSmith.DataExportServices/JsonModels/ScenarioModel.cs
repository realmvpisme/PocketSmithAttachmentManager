using System;
using System.Text.Json.Serialization;

namespace PocketSmith.DataExportServices.JsonModels
{
    public class ScenarioModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("account_id")]
        public long? AccountId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("interest_rate")]
        public decimal? InterestRate { get; set; }
        [JsonPropertyName("interest_rate_repeat_id")]
        public long? InterestRateRepeatId { get; set; } 
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("is_net_worth")]
        public bool? IsNetWorth { get; set; }
        [JsonPropertyName("minimum_value")]
        public decimal? MinimumValue { get; set; }
        [JsonPropertyName("maximum_value")]
        public decimal? MaximumValue { get; set; }
        [JsonPropertyName("achieve_date")]
        public DateTime? AchieveDate { get; set; }
        [JsonPropertyName("starting_balance")]
        public decimal? StartingBalance { get; set; }
        [JsonPropertyName("starting_balance_date")]
        public DateTime StartingBalanceDate { get; set; }
        [JsonPropertyName("closing_balance")]
        public decimal? ClosingBalance { get; set; }
        [JsonPropertyName("closing_balance_date")]
        public DateTime? ClosingBalanceDate { get; set; }
        [JsonPropertyName("current_balance")]
        public decimal? CurrentBalance { get; set; }
        [JsonPropertyName("current_balance_in_base_currency")]
        public decimal? CurrentBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("current_balance_exchange_rate")]
        public string CurrentBalanceExchangeRate { get; set; }
        [JsonPropertyName("current_balance_date")]
        public DateTime? CurrentBalanceDate { get; set; }
        [JsonPropertyName("safe_balance")]
        public decimal? SafeBalance { get; set; }
        [JsonPropertyName("safe_balance_in_base_currency")]
        public decimal? SafeBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}