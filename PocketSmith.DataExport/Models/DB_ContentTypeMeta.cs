using System.Collections;
using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_ContentTypeMeta : ModelBase<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }

        public ICollection<DB_Attachment> Attachments { get; set; }
    }
}