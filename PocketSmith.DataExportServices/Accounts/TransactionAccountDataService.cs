using System.Threading.Tasks;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts
{
    public class TransactionAccountDataService : DataServiceBase<TransactionAccountModel, DB_TransactionAccount, long>
    {
        public TransactionAccountDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }

    }
}