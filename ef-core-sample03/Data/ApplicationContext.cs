using System;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasteringEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Server=localhost,1433;Database=MasteringEFCoreDB-03;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true";
            optionsBuilder
                .UseSqlServer(strConnection)
                 // .UseSqlServer(strConnection, 
                //               p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Department>().HasQueryFilter(p => !p.Deleted);
        // }
    }
}