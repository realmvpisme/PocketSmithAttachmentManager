using System.Collections.Generic;
using System.Threading.Tasks;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;

namespace PocketSmithAttachmentManager.WebServices
{
    public class BudgetService : WebServiceBase<BudgetEventModel, long>
    {
        public BudgetService() : base(PocketSmithUri.BASE_URI)
        {

        }

        public async Task<List<BudgetEventModel>> GetAll()
        {

        }
    }
}