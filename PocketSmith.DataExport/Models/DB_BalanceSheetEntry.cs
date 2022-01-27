using System;

namespace PocketSmith.DataExport.Models
{
    public class DB_BalanceSheetEntry : ModelBase<long>
    {
        public decimal AccountBalance { get; set; }
        public DateTime FirstOfMonthDate { get; set; }
        public DateTime? OriginalTransactionDate { get; set; }
        public long? TransactionId { get; set; }
        public DB_Transaction Transaction { get; set; }
        public long TransactionAccountId { get; set; }
        public DB_TransactionAccount TransactionAccount { get; set; }
    }
}