using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Budget
{
    public class BudgetEventDataService : DataServiceBase<BudgetEventModel, DB_BudgetEvent>
    {
        public BudgetEventDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}