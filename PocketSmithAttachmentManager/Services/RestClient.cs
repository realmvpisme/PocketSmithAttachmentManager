using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Services
{
    public class RestClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        public string CurrentPageUri { get; set; }
        public string FirstPageUri { get; private set; }
        public string PreviousPageUri { get; set; }
        public string NextPageUri { get; set; }
        public string LastPageUri { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPageNumber { get; private set; }

        public RestClient(string baseUri)
        {
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            _baseUri = baseUri;
            CurrentPageUri = baseUri;
        }

        public async Task<string> Get(string uri)
        {
            var httpResponse = await _httpClient.GetAsync(uri);

            try
            {
                var links = httpResponse.Headers.GetValues("Link").FirstOrDefault();

                FirstPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"first\\\")").Value;
                NextPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"next\\\")").Value;
                LastPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"last\\\")").Value;

                if (TotalPages == 0)
                {
                    TotalPages = Convert.ToInt32(Regex.Match(LastPageUri, "(?<=\\?page=)\\d+").Value);
                }

                CurrentPageNumber = Convert.ToInt32(Regex.Match(CurrentPageUri, "(?<=\\?page=)\\d+").Value);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No additional pages were found \n");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}