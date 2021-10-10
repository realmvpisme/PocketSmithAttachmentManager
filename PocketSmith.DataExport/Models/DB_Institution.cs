using System.Collections;
using System.Collections.Generic;

namespace PocketSmith.DataExport.Models
{
    public class DB_Institution : ModelBase<long>
    {
        public string Title { get; set; }
        public string CurrencyCode { get; set; }

        public ICollection<DB_TransactionAccount> TransactionAccounts { get; set; }
    }
}