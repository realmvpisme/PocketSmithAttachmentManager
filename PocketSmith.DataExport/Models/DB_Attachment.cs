namespace PocketSmith.DataExport.Models
{
    public class DB_Attachment : ModelBase<long>
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public string OriginalUrl { get; set; }
        public long? VariantId { get; set; }
        public DB_Variant Variants { get; set; }
        public long? ContentTypeMetaId { get; set; }
        public DB_ContentTypeMeta ContentTypeMeta { get; set; }
        public long? TransactionId { get; set; }
        public DB_Transaction Transaction { get; set; }

    }
}