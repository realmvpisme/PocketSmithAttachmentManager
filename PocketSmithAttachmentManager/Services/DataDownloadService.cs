using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;

namespace PocketSmithAttachmentManager.Services
{
    public class DataDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly Type _parentType;
        private PocketSmithDbContext _context;
        private TransactionService _transactionService;

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
        }

        public async Task DownloadAllData()
        {
            if (_context == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A database file must be loaded before downloading data.");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Downloading transactions...");
        }

        public async Task<bool> LoadDatabase(string filePath)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder();
            contextOptionsBuilder.UseSqlite($"Data Source={filePath}");
            await using var _context = new PocketSmithDbContext(contextOptionsBuilder.Options);

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
                await _context.Database.MigrateAsync();
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

        private async Task downloadTransactions()
        {

        }

    }
}