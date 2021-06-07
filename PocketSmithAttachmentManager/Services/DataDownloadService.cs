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
        }

        public async Task DownloadAllData()
        {

        }

        public async Task<bool> LoadDatabase(string filePath)
        { 
            var contextOptionsBuilder = new DbContextOptionsBuilder();
            contextOptionsBuilder.UseSqlite($"Data Source={filePath}");
            await using var _context = new PocketSmithDbContext(contextOptionsBuilder.Options);

            await _context.Database.MigrateAsync();

            return true;
        }

    }
}