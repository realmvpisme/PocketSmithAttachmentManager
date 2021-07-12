using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.WebServices
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
        }

        public async Task<string> Get(string uri)
        {
            CurrentPageUri = uri;
            var httpResponse = await _httpClient.GetAsync(uri);

            try
            {
                string links;
                try
                {
                    links = httpResponse.Headers.GetValues("Link").FirstOrDefault();

                }
                catch (Exception e)
                {
                    links = null;
                }


                if (links != null)
                {
                    FirstPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"first\\\")").Value;
                    NextPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"next\\\")").Value;
                    LastPageUri = Regex.Match(links, "(?<=<)[^<]+(?=>;\\srel=\\\"last\\\")").Value;
                }
                else
                {
                    FirstPageUri = uri;
                    CurrentPageUri = uri;
                    LastPageUri = uri;
                }

                if (TotalPages == 0 && links != null)
                {
                    TotalPages = Convert.ToInt32(Regex.IsMatch(LastPageUri, "(?<=\\?page=)\\d+") ? 
                        Regex.Match(LastPageUri, "(?<=\\?page=)\\d+").Value : 
                        Regex.Match(LastPageUri, "(?<=\\&page=)\\d+").Value);
                }
                else
                {
                    TotalPages = 1;
                }

                string currentPageNumberString = null;
                if (Regex.IsMatch(CurrentPageUri, "(?<=\\?page=)\\d+"))
                {
                    currentPageNumberString = Regex.Match(CurrentPageUri, "(?<=\\?page=)\\d+").Value;
                }
                else
                {
                    currentPageNumberString = Regex.Match(CurrentPageUri, "(?<=\\&page=)\\d+").Value;
                }
                CurrentPageNumber = !string.IsNullOrEmpty(currentPageNumberString) ? Convert.ToInt32(currentPageNumberString) : 1;
            }
            catch (Exception e)
            {
            }

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}