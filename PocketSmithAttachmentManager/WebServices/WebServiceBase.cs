using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.WebServices.Extensions;
using ShellProgressBar;

namespace PocketSmithAttachmentManager.WebServices
{
    public abstract class WebServiceBase<TJsonModel, TId>
    {
        protected readonly HttpClient HttpClient;
        protected readonly RestClient RestClient;
        protected readonly JsonSerializerOptions SerializerOptions;
        private readonly string _entityType;
        private readonly string _baseUri;


        protected WebServiceBase(string baseUri, string entityType)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            RestClient = new RestClient(baseUri);

            SerializerOptions = new JsonSerializerOptions();
            SerializerOptions.Converters.Add(new JsonInt32Converter());
            SerializerOptions.Converters.Add(new JsonBoolConverter());
            SerializerOptions.Converters.Add(new JsonDecimalConverter());

            _entityType = entityType;
            _baseUri = baseUri;
        }

        public virtual async Task<List<TJsonModel>> GetAll()
        {
            var uri = _baseUri;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var apiEntityList = new List<TJsonModel>();
            var httpResponse = await RestClient.Get(uri);
            var apiEntities = JsonSerializer.Deserialize<List<TJsonModel>>(httpResponse, SerializerOptions);

            apiEntities.ForEach(x => apiEntityList.Add(x));

            var progressBarOptions = new ProgressBarOptions()
            {
                CollapseWhenFinished = true,
                ForegroundColor = ConsoleColor.White
            };

            using var progressBar = new ProgressBar(RestClient.TotalPages, $"Downloading {_entityType}...", progressBarOptions);

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
                apiEntities = JsonSerializer.Deserialize<List<TJsonModel>>(httpResponse, SerializerOptions);

                apiEntities.ForEach(x => apiEntityList.Add(x));

                progressBar.Tick();
            } while (RestClient.CurrentPageUri != RestClient.LastPageUri);


            return apiEntityList;
        }
        public abstract Task<TJsonModel> GetById(TId id);
    }
}