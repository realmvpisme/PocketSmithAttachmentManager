namespace PocketSmith.DataExport.Models
{
    public class DB_Institution : ModelBase<long>
    {
        public string Title { get; set; }
        public string CurrencyCode { get; set; }
    }
}