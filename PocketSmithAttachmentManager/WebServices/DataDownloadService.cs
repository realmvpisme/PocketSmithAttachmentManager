using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;

namespace PocketSmithAttachmentManager.WebServices
{
    public class DataDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly Type _parentType;
        private TransactionService _transactionService;
        private ContextFactory _contextFactory;
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

            Console.Clear();

            var transactions = await _transactionService.GetAllTransactions();

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