using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Institutions
{
    public class InstitutionDataService : DataServiceBase<InstitutionModel, DB_Institution, long>
    {
        public InstitutionDataService(string dataFilePath) : base(dataFilePath)
        {

        }
    }
}