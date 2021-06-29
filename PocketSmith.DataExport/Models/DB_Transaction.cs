using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Transaction : ModelBase<long>
    {
        public string Payee { get; set; }
        public string OriginalPayee { get; set; }
        public string Date { get; set; }
        public string UploadSource { get; set; }
        public decimal ClosingBalance { get; set; }
        public int? CheckNumber { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInBaseCurrency { get; set; }
        public string Type { get; set; }
        public bool? IsTransfer { get; set; }
        public bool? NeedsReview { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string[] Labels { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public long? CategoryId { get; set; }
        public DB_Category Category { get; set; }
        public long? AccountId { get; set; }
        public DB_Account Account { get; set; }

        public ICollection<DB_Attachment> Attachments { get; set; }
    }
}