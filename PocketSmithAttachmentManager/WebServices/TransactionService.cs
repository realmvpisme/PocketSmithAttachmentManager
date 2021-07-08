using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmithAttachmentManager.Constants;
using PocketSmithAttachmentManager.WebServices.Extensions;
using ShellProgressBar;

namespace PocketSmithAttachmentManager.WebServices
{
    public class TransactionService : WebServiceBase<TransactionModel, long>
    {

        public TransactionService() : base(PocketSmithUri.TRANSACTION_SEARCH, EntityType.CATEGORIES)
        {

        }

        public override async Task<List<TransactionModel>> GetAll()
        {
            var uri = PocketSmithUri.TRANSACTIONS;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"]);

            var transactionList = new List<TransactionModel>();
            var httpResponse = await RestClient.Get(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, SerializerOptions);

            transactions.ForEach(t => transactionList.Add(t));

            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(RestClient.TotalPages, "Downloading Transactions", ConsoleColor.White);

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
                transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, SerializerOptions);
                transactions.ForEach(t => transactionList.Add(t));

                progressBar.Tick();
            } while (RestClient.CurrentPageUri != RestClient.LastPageUri);

            progressBar.Dispose();

            return transactionList;
        }
        public async Task<List<TransactionModel>> GetByAmount(decimal amount)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", amount.ToString());

            var httpResponse = await RestClient.Get(uri);

            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, SerializerOptions);

            return indexTransactions(transactions);
        }
        public async Task<List<TransactionModel>> GetByPayee(string payee)
        {
            var uri = PocketSmithUri.TRANSACTION_SEARCH;

            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{searchTerm}", payee);

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, SerializerOptions);

            return indexTransactions(transactions);
        }

        public async Task<List<TransactionModel>> GetByDate(DateTime startDate, DateTime? endDate = null)
        {
            endDate ??= startDate;

            var uri = PocketSmithUri.TRANSACTIONS_BY_DATE;
            uri = uri.Replace("{userId}", ConfigurationManager.AppSettings["userId"])
                .Replace("{startDate}", startDate.Date.ToString("yyyy-MM-dd"))
                .Replace("{endDate}", endDate.Value.Date.ToString("yyyy-MM-dd"));

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(httpResponse, SerializerOptions);

            return indexTransactions(transactions);
        }

        public override async Task<TransactionModel> GetById(long id)
        {
            var uri = PocketSmithUri.TRANSACTION_BY_ID;
            uri = uri.Replace("{id}", id.ToString());

            var httpResponse = await HttpClient.GetStringAsync(uri);
            var transaction = JsonSerializer.Deserialize<TransactionModel>(httpResponse, SerializerOptions);
            return transaction;
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