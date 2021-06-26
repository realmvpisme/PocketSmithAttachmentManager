using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using PocketSmith.DataExport.Models;
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
            var uri = PocketSmithUri.BUDGET_EVENTS_BY_DATE;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{startDate}", "2000-01-01")
                .Replace("{endDate}", $"{DateTime.Now.Year}-12-31");



        }
    }
}