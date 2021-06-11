using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;
using PocketSmithAttachmentManager.WebServices.Extensions;
using ShellProgressBar;

namespace PocketSmithAttachmentManager.WebServices
{
    public class TransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly RestClient _restClient;

        public TransactionService()
        {
            _restClient = new RestClient(PocketSmithUri.TRANSACTION_SEARCH);
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new JsonInt32Converter());
            _serializerOptions.Converters.Add(new JsonBoolConverter());
            _serializerOptions.Converters.Add(new JsonDecimalConverter());
        }

        public async Task<List<TransactionModel>> GetAllTransactions()
        {
            var uri = PocketSmithUri.TRANSACTIONS;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var transactionList = new List<TransactionModel>();
            var httpResponse = await _restClient.Get(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);

            transactions.ForEach(t => transactionList.Add(t));

            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(_restClient.TotalPages, "Downloading Transactions", ConsoleColor.White);

            progressBar.Tick();

            do
            {
                if (!string.IsNullOrEmpty(_restClient.CurrentPageUri))
                {
                    _restClient.PreviousPageUri = _restClient.CurrentPageUri;
                }

                if (!string.IsNullOrEmpty(_restClient.NextPageUri))
                {
                    _restClient.CurrentPageUri = _restClient.NextPageUri;
                }

                httpResponse = await _restClient.Get(_restClient.CurrentPageUri);
                transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);
                transactions.ForEach(t => transactionList.Add(t));

                progressBar.Tick();
                //ToDo: Remove transaction count criteria.
            } while (_restClient.CurrentPageUri != _restClient.LastPageUri && transactionList.Count < 200);

            progressBar.Dispose();

            return transactionList;
        }
        public async Task<List<TransactionModel>> GetTransactionsByAmount(decimal amount)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", amount.ToString());

            var httpResponse = await _restClient.Get(uri);

            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);

            return indexTransactions(transactions);
        }
        public async Task<List<TransactionModel>> GetTransactionsByPayee(string payee)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", payee);

            var httpResponse = await _httpClient.GetStringAsync(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);

            return indexTransactions(transactions);
        }

        public async Task<List<TransactionModel>> GetTransactionsByDate(DateTime startDate, DateTime? endDate = null)
        {
            endDate ??= startDate;

            var uri = PocketSmithUri.TRANSACTIONS_BY_DATE;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{startDate}", startDate.Date.ToString("yyyy-MM-dd"))
                .Replace("{endDate}", endDate.Value.Date.ToString("yyyy-MM-dd"));

            var httpResponse = await _httpClient.GetStringAsync(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);

            return indexTransactions(transactions);
        }


        private List<TransactionModel> indexTransactions(List<TransactionModel> transactions)
        {
            int transactionCount = 1;
            foreach (var transaction in transactions)
            {
                transaction.Index = transactionCount;
                transactionCount++;
            }

            return transactions;
        }

    }

   
}