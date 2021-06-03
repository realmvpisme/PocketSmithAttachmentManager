using System.Collections;
using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Variant : ModelBase
    {
        public string ThumbUrl { get; set; }
        public string LargeUrl { get; set; }

        public ICollection<DB_Attachment> Attachments { get; set; }
    }
}