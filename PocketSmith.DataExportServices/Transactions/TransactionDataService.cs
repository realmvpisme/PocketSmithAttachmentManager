using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Transactions
{
    public class TransactionDataService : DataServiceBase<TransactionModel, DB_Transaction, long>
    {
        public TransactionDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}