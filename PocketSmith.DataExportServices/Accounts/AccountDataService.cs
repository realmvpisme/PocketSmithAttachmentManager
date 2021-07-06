using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts
{
    public class AccountDataService : DataServiceBase<TransactionAccountModel, DB_TransactionAccount, long>
    {
        public AccountDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}