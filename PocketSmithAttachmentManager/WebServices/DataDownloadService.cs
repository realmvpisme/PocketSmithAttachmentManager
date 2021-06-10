using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExportServices.Accounts;
using PocketSmith.DataExportServices.JsonModels;
using PocketSmith.DataExportServices.Transactions;
using ShellProgressBar;

namespace PocketSmithAttachmentManager.WebServices
{
    public class DataDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly Type _parentType;
        private readonly TransactionService _transactionService;
        private readonly ContextFactory _contextFactory;
        private string _databaseFilePath;

        public DataDownloadService(Type parentType)
        {
            _httpClient = new HttpClient();
            _httpClient
                .DefaultRequestHeaders
                .Add("X-Developer-Key", ConfigurationManager.AppSettings["apiKey"]);
            _httpClient
                .DefaultRequestHeaders
                .Add("Accept", "application/json");

            _parentType = parentType;

            _transactionService = new TransactionService();
            _contextFactory = new ContextFactory();
        }

        public async Task DownloadAllData()
        {
            var context = _contextFactory.Create(_databaseFilePath);
            var transactionDataService = new TransactionDataService(_databaseFilePath);
            var accountDataService = new AccountDataService(_databaseFilePath);

            Console.Clear();
            
            var transactions = await _transactionService.GetAllTransactions();

            var progressBarOptions = new ProgressBarOptions()
            {
                ProgressCharacter = ('-'),
                DisplayTimeInRealTime = false
            };
            using var progressBar = new ProgressBar(transactions.Count, "Adding Transactions to Database", ConsoleColor.White);

            

            foreach (var transaction in transactions)
            {
                progressBar.Tick();
                List<TransactionModel> existingTransactions = new List<TransactionModel>();
                if (await transactionDataService.Exists(transaction.Id))
                {
                    //ToDo: Determine best way to check for changes and update if needed.
                    existingTransactions.Add(transaction);
                }
                else
                {
                    await transactionDataService.Create(transaction);
                }

                if (transaction.TransactionAccount != null)
                {
                    List<AccountModel> existingAccounts = new List<AccountModel>();
                    if (await accountDataService.Exists(transaction.TransactionAccount.AccountId))
                    {
                        existingAccounts.Add(transaction.TransactionAccount);
                    }
                    else
                    {
                        await accountDataService.Create(transaction.TransactionAccount);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All transactions downloaded successfully!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task<bool> LoadDatabase(string filePath)
        {
            _databaseFilePath = filePath;

            await using var context = _contextFactory.Create(filePath);

            bool fileExists = File.Exists(filePath);

            Console.Clear();
            if (fileExists)
            {
                Console.WriteLine("Loading database file...");
            }
            else
            {
                Console.WriteLine("Database file does not exist. Creating new database file...");
            }

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Database file failed to load.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database file loaded successfully.");
            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }

    }
}