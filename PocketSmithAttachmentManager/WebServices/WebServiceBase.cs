using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmithAttachmentManager.WebServices.Extensions;

namespace PocketSmithAttachmentManager.WebServices
{
    public abstract class WebServiceBase<TJsonModel, TId>
    {
        protected readonly HttpClient HttpClient;
        protected readonly RestClient RestClient;
        protected readonly JsonSerializerOptions SerializerOptions;


        protected WebServiceBase(string baseUri)
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
        }

        public abstract Task<List<TJsonModel>> GetAll();
        public abstract Task<TJsonModel> GetById(TId id);
    }
}