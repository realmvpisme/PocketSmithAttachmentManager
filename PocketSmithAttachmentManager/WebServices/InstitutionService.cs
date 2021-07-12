using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.WebServices
{
    public class InstitutionService : WebServiceBase<InstitutionModel, long>
    {
        public InstitutionService() : base(PocketSmithUri.INSTITUTIONS_ALL, EntityType.INSTITUTIONS)
        {
        }

        public override async Task<List<InstitutionModel>> GetAll()
        {
            var uri = PocketSmithUri.INSTITUTIONS_ALL;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var institutionList = new List<InstitutionModel>();
            var httpResponse = await RestClient.Get(uri);
            var institutions = JsonSerializer.Deserialize<List<InstitutionModel>>(httpResponse);

            institutions.ForEach(i => institutionList.Add(i));

            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(RestClient.TotalPages, "Downloading Institutions", ConsoleColor.White);

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
                institutions = JsonSerializer.Deserialize<List<InstitutionModel>>(httpResponse, SerializerOptions);
                institutions.ForEach(t => institutions.Add(t));

                progressBar.Tick();
            } while (RestClient.CurrentPageUri != RestClient.LastPageUri);

            progressBar.Dispose();
            Console.Clear();

            return institutionList;
        }

        public override async Task<InstitutionModel> GetById(long id)
        {
            var uri = PocketSmithUri.TRANSACTION_BY_ID;
            uri = uri.Replace("{id}", id.ToString());

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var institution = JsonSerializer.Deserialize<InstitutionModel>(httpResponse);
            return institution;
        }
    }
}