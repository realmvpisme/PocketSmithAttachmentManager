using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Categories
{
    public class CategoryDataService : DataServiceBase<CategoryModel, DB_Category>
    {
        public CategoryDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }
    }
}