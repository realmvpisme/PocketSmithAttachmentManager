using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;

namespace PocketSmith.DataExportServices.JsonModels
{
    public class AccountModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonPropertyName("current_balance")]
        public decimal CurrentBalance { get; set; }
        [JsonPropertyName("current_balance_in_base_currency")]
        public decimal CurrentBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("current_balance_exchange_rate")]
        public decimal? CurrentBalanceExchangeRate { get; set; }
        [JsonPropertyName("current_balance_date")]
        public DateTime? CurrentBalanceDate { get; set; }
        [JsonPropertyName("safe_balance")]
        public decimal? SafeBalance { get; set; }
        [JsonPropertyName("safe_balance_in_base_currency")]
        public decimal? SafeBalanceInBaseCurrency { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("is_net_worth")]
        public string IsNetWorth { get; set; }
        [JsonPropertyName("primary_transaction_account")]
        public TransactionAccountModel PrimaryTransactionAccount { get; set; }
        [JsonPropertyName("primary_scenario")]
        public ScenarioModel PrimaryScenario { get; set; }
        [JsonPropertyName("transaction_accounts")]
        public List<TransactionAccountModel> TransactionAccounts { get; set; }
        [JsonPropertyName("scenarios")]
        public List<ScenarioModel> Scenarios { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}