using System;

namespace PocketSmith.DataExport.Models
{
    public class ModelBase<TEntityId>
    {
        public TEntityId Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}