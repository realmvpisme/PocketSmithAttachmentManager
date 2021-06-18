using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Category : ModelBase
    {
        public string Title { get; set; }
        public string Color { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsBill { get; set; }
        public string RefundBehaviour { get; set; }
        public bool RollUp { get; set; }
        public long? ParentId { get; set; }

    }
}