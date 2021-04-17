using System;
using PocketSmithAttachmentManager.Constants;
using PocketSmithAttachmentManager.Models;
using PocketSmithAttachmentManager.Services.Extensions;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace PocketSmithAttachmentManager.Services
{
    public class TransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public TransactionService()
        {
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

        public async Task<List<TransactionModel>> GetTransactionsByAmount(decimal amount)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", amount.ToString());

            var httpResponse = await _httpClient.GetStringAsync(uri);

            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, _serializerOptions);

            return indexTransactions(transactions);
        }
        public async Task<List<TransactionModel>> GetTransactionsByPayee(string payee)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", payee);

            var httpResponse = await _httpClient.GetStringAsync(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse);

            return indexTransactions(transactions);
        }

        public async Task<List<TransactionModel>> GetTransactionsByDate(DateTime startDate, DateTime? endDate = null)
        {
            throw new NotImplementedException();
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