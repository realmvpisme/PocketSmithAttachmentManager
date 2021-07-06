using System;

namespace PocketSmith.DataExport.Models
{
    public class DB_AccountBalance : ModelBase<long>
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public long AccountId { get; set; }
        public DB_Account Account { get; set; }
    }
}