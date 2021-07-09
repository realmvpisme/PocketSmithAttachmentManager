using System;
using PocketSmith.DataExport.Models;

namespace PocketSmith.DataExportServices.JsonModels
{
    public class AccountBalanceModel
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public long AccountId { get; set; }
    }
}