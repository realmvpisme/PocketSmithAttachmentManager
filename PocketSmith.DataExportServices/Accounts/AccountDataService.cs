using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts
{
    public class AccountDataService : DataServiceBase<AccountModel, DB_Account, long>
    {
        public AccountDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}