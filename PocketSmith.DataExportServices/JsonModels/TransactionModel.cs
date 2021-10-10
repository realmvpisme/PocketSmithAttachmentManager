using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PocketSmith.DataExportServices.JsonModels
{
    public class TransactionModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("payee")]
        public string Payee { get; set; }
        [JsonPropertyName("original_payee")]
        public string OriginalPayee { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("upload_source")]
        public string UploadSource { get; set; }
        [JsonPropertyName("category")]
        public CategoryModel Category { get; set; }
        [JsonPropertyName("closing_balance")]
        public decimal ClosingBalance { get; set; }
        [JsonPropertyName("cheque_number")]
        public int? CheckNumber { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("amount_in_base_currency")]
        public decimal AmountInBaseCurrency { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("is_transfer")]
        public bool? IsTransfer { get; set; }
        [JsonPropertyName("needs_review")]
        public bool? NeedsReview { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("note")]
        public string Note { get; set; }
        [JsonPropertyName("labels")]
        public string [] Labels { get; set; }
        [JsonPropertyName("transaction_account")]
        public TransactionAccountModel TransactionAccount { get; set; }

        private DateTime _createdAt;
        [JsonPropertyName("created_at")]
        public string CreatedAt
        {
            get => _createdAt.ToLongDateString();
            set => _createdAt = DateTime.Parse(value).ToUniversalTime();
        }

        private DateTime _updatedAt;
        [JsonPropertyName("updated_at")]
        public string UpdatedAt
        {
            get => _updatedAt.ToLongDateString();
            set => _updatedAt = DateTime.Parse(value).ToUniversalTime();
        }

        [JsonIgnore]
        public int Index { get; set; }
        [JsonIgnore]
        public List<AttachmentModel> Attachments { get; set; }
    }
}