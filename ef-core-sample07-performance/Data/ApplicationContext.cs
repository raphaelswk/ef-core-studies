using System;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasteringEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MyFunction> MyFunctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Server=localhost,1433;Database=MasteringEFCoreDB-06;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true";
            optionsBuilder
                .UseSqlServer(strConnection)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Shadow Property Configuration
            modelBuilder.Entity<MyFunction>(conf =>
            {
                conf.Property<string>("MyShadowProperty")
                    .HasColumnType("VARCHAR(100)")
                    .HasDefaultValueSql("'TEST'");
            });
        }
    }
}
