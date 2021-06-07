using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using PocketSmith.DataExport.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DB_Account>().ToTable("Accounts");
            modelBuilder.Entity<DB_Attachment>().ToTable("Attachments");
            modelBuilder.Entity<DB_Category>().ToTable("Categories");
            modelBuilder.Entity<DB_ContentTypeMeta>().ToTable("ContentTypeMetas");
            modelBuilder.Entity<DB_Institution>().ToTable("Institutions");
            modelBuilder.Entity<DB_Transaction>().ToTable("Transactions");
            modelBuilder.Entity<DB_Variant>().ToTable("Variants");

            modelBuilder.Entity<DB_Transaction>().Property(t => t.Labels)
                .HasConversion(l => JsonSerializer.Serialize(l));
        }

    }
}
