using Microsoft.EntityFrameworkCore;

namespace PocketSmith.DataExport
{
    public class ContextFactory
    {
        public PocketSmithDbContext Create(string filePath)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder();
            contextOptionsBuilder.UseSqlite($"Data Source={filePath}");
            return new PocketSmithDbContext(contextOptionsBuilder.Options);
        }
    }
}