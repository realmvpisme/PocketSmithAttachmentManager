using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PocketSmith.DataExport
{
    public class PocketSmithDbContext : DbContext
    {
        public PocketSmithDbContext() : base()
        {

        }
        public PocketSmithDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DB_Account> Accounts { get; set; }
        public DbSet<DB_Attachment> Attachments { get; set; }
        public DbSet<DB_Category> Categories { get; set; }
        public DbSet<DB_ContentTypeMeta> ContentTypeMetas { get; set; }
        public DbSet<DB_Institution> Institutions { get; set; }
        public DbSet<DB_Transaction> Transactions { get; set; }
        public DbSet<DB_Variant> Variants { get; set; }
        public DbSet<DB_BudgetEvent> BudgetEvents { get; set; }
        public DbSet<DB_Scenario> Scenarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DB_Account>().ToTable("Accounts");
            modelBuilder.Entity<DB_Attachment>().ToTable("Attachments");
            modelBuilder.Entity<DB_Category>().ToTable("Categories");
            modelBuilder.Entity<DB_ContentTypeMeta>().ToTable("ContentTypeMetas");
            modelBuilder.Entity<DB_Institution>().ToTable("Institutions");
            modelBuilder.Entity<DB_Transaction>().ToTable("Transactions");
            modelBuilder.Entity<DB_Variant>().ToTable("Variants");
            modelBuilder.Entity<DB_BudgetEvent>().ToTable("BudgetEvents");
            modelBuilder.Entity<DB_Scenario>().ToTable("Scenarios");


            modelBuilder.Entity<DB_Account>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<DB_Attachment>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<DB_Category>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<DB_Institution>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<DB_Transaction>()
                .Property(x => x.Id)
                .ValueGeneratedNever();



            modelBuilder.Entity<DB_Transaction>().Property(t => t.Labels)
                .HasConversion(l => JsonSerializer.Serialize(l, default), 
                    l => JsonSerializer.Deserialize<string []>(l, default));
        }

    }
}
