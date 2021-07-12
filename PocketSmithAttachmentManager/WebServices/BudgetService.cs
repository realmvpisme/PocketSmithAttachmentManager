using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;
using ShellProgressBar;

namespace PocketSmithAttachmentManager.WebServices
{
    public class BudgetService : WebServiceBase<BudgetEventModel, long>
    {
        public BudgetService() : base(PocketSmithUri.BASE_URI, EntityType.BUDGET_EVENTS)
        {

        }

        public override async Task<List<BudgetEventModel>> GetAll()
        {
            var uri = PocketSmithUri.BUDGET_EVENTS_BY_DATE;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{startDate}", "2000-01-01")
                .Replace("{endDate}", $"{DateTime.Now.Year}-12-31");

            var budgetEventList = new List<BudgetEventModel>();
            var httpResponse = await RestClient.Get(uri);
            var budgetEvents = JsonSerializer.Deserialize<List<BudgetEventModel>>(httpResponse, SerializerOptions);

            budgetEvents.ForEach(b => budgetEventList.Add(b));

            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(RestClient.TotalPages, "Downloading Budget Events", ConsoleColor.White);
            
            progressBar.Tick();

            do
            {
                if (!string.IsNullOrEmpty(RestClient.CurrentPageUri))
                {
                    RestClient.PreviousPageUri = RestClient.CurrentPageUri;
                }

                if (!string.IsNullOrEmpty(RestClient.NextPageUri))
                {
                    RestClient.CurrentPageUri = RestClient.NextPageUri;
                }

                httpResponse = await RestClient.Get(RestClient.CurrentPageUri);
                budgetEvents = JsonSerializer.Deserialize<List<BudgetEventModel>>(httpResponse, SerializerOptions);
                budgetEvents.ForEach(t => budgetEventList.Add(t));

                progressBar.Tick();
            } while (RestClient.CurrentPageUri != RestClient.LastPageUri);

            progressBar.Dispose();

            return budgetEventList;
        }

        public override async Task<BudgetEventModel> GetById(long id)
        {
            throw new NotImplementedException();
        }
    }
}