using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PocketSmith.DataExport;

namespace PocketSmithAttachmentManager.DesignTimeServices
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PocketSmithDbContext>
    {
        public PocketSmithDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PocketSmithDbContext>();
            var connectionString = $"Data Source=database.db";
            builder.UseSqlite(connectionString);
            return new PocketSmithDbContext(builder.Options);
        }
    }
}