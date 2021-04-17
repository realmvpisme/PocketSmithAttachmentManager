using System;
using System.Text.Json.Serialization;

namespace PocketSmithAttachmentManager.Models
{
    public class AccountModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("account_id")]
        public long AccountId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("number")]
        public string Number { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("is_net_worth")]
        public bool? IsNetWorth { get; set; }
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonPropertyName("current_balance")]
        public decimal CurrentBalance { get; set; }
        [JsonPropertyName("current_balance_in_base_currency")]
        public decimal CurrentBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("current_balance_exchange_rate")]
        public string CurrentBalanceExchangeRate { get; set; }
        [JsonPropertyName("current_balance_date")]
        public DateTime CurrentBalanceDate { get; set; }
        [JsonPropertyName("safe_balance")]
        public decimal? SafeBalance { get; set; }
        [JsonPropertyName("safe_balance_in_base_currency")]
        public decimal? SafeBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("starting_balance")]
        public decimal? StartingBalance { get; set; }
        [JsonPropertyName("starting_balance_date")]
        public DateTime StartingBalanceDate { get; set; }
        [JsonPropertyName("institution")]
        public InstitutionModel Institution { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}