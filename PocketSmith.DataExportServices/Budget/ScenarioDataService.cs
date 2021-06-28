using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Budget
{
    public class ScenarioDataService : DataServiceBase<ScenarioModel, DB_Scenario>
    {
        public ScenarioDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}