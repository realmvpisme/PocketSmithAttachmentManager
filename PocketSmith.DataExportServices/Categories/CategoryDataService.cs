using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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