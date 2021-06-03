using System;

namespace PocketSmith.DataExport.Models
{
    public class ModelBase
    {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}